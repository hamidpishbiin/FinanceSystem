using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Bootstrap;
using FinanceSystem.Persistance.Mappings;
using FinanceSystem.Psp;
using FinanceSystem.Presentation;
using FinanceSystem.Security;
using Serilog;
using Shared.Core;
using Shared.Serilog;
using Scalar.AspNetCore;

const string appName = "FinanceSystem.Service";
Log.Logger = SerilogFactory.CreateLogger(appName);
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.Services.RegisterLocalization();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAuthorization();
    builder.Services.AddHttpClient();
    builder.Services.AddPspHttpClients();
    builder.Services.AddSerilog();

    if (builder.Environment.IsProduction())
    {
        var envPath = Environment.GetEnvironmentVariable("FINANCESYSTEM_ENV_FILE") ?? "/etc/financesystem/.env";

        if (!File.Exists(envPath))
        {
            throw new InvalidOperationException($"Env file not found at path '{envPath}'");
        }

        DotNetEnv.Env.Load(envPath);

        builder.Configuration.AddEnvironmentVariables();
    }

    builder.Services.AddOpenApi();

    builder.AddCORS();

    builder.Services.AddScoped<IUserResolver, UserResolver>();

    builder.AddAuthenticationByJwtToken();


    builder.Services.AddOptions<ConnectionStrings>()
        .Bind(builder.Configuration.GetSection(ConnectionStrings.SectionName))
        .Validate(
            c => !string.IsNullOrWhiteSpace(c.DefaultConnection),
            $"{ConnectionStrings.SectionName}:DefaultConnection is required.")
        .Validate(
            c => !string.IsNullOrWhiteSpace(c.ReadOnlyConnection),
            $"{ConnectionStrings.SectionName}:ReadOnlyConnection is required.")
        .ValidateOnStart();
    builder.Services.AddOptions<PspOptions>()
        .Bind(builder.Configuration.GetSection(PspOptions.SectionName))
        .Validate(
            o => IsConfigured(o.Saman),
            $"{PspOptions.SectionName}:Saman needs absolute TopUpCallBackUrl and BaseUrl, MerchantId, TerminalId, CredentialsRef and a positive TimeoutInSeconds.")
        .Validate(
            o => IsConfigured(o.BehPardakht),
            $"{PspOptions.SectionName}:BehPardakht needs absolute TopUpCallBackUrl and BaseUrl, MerchantId, TerminalId, CredentialsRef and a positive TimeoutInSeconds.")
        .ValidateOnStart();

    static bool IsConfigured(PspSettings? psp) =>
        psp is not null
        && Uri.TryCreate(psp.TopUpCallBackUrl, UriKind.Absolute, out _)
        && Uri.TryCreate(psp.BaseUrl, UriKind.Absolute, out _)
        && !string.IsNullOrWhiteSpace(psp.MerchantId)
        && !string.IsNullOrWhiteSpace(psp.TerminalId)
        && !string.IsNullOrWhiteSpace(psp.CredentialsRef)
        && psp.TimeoutInSeconds > 0;
    var connectionStrings = builder.Configuration.GetSection(ConnectionStrings.SectionName).Get<ConnectionStrings>()
        ?? throw new InvalidOperationException($"Configuration section '{ConnectionStrings.SectionName}' is missing.");

    if (string.IsNullOrWhiteSpace(connectionStrings.DefaultConnection))
        throw new InvalidOperationException($"{ConnectionStrings.SectionName}:DefaultConnection is required.");

    if (string.IsNullOrWhiteSpace(connectionStrings.ReadOnlyConnection))
        throw new InvalidOperationException($"{ConnectionStrings.SectionName}:ReadOnlyConnection is required.");

    builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
        builder.AddModule(connectionStrings.DefaultConnection, connectionStrings.ReadOnlyConnection, typeof(ProductMapping).Assembly));

    builder.Host.UseSerilog();

    builder.Services.AddHealthChecks();





    var app = builder.Build();



    var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
    app.UseRequestLocalization(locOptions.Value);
    app.UseCors(RegisterCORSExtensions.CORSName);
    app.UseRouting();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandlerMiddleware();
    }

    //if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle($"{appName} API")
                .WithTheme(ScalarTheme.BluePlanet)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

    }

    app.MapHealthChecks("/health/live");

    app.MapHealthChecks("/health/ready",
        new HealthCheckOptions
        {
            Predicate = x => x.Tags.Contains("ready")
        });

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host of {App} terminated unexpectedly", appName);
    throw;
}
finally
{
    Log.Information("Shutting down web host for {App}", appName);
    Log.CloseAndFlush();
}
