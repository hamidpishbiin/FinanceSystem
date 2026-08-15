using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using ProductManagement.Bootstrap;
using ProductManagement.Persistance.Mappings;
using ProductManagement.Presentation;
using ProductManagement.Security;
using Serilog;
using Shared.Core;
using Shared.Serilog;
using Scalar.AspNetCore;

const string appName = "ProductManagement.Service";
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
    builder.Services.AddSerilog();

    if (builder.Environment.IsProduction())
        builder.Configuration.AddEnvironmentVariables();

    builder.Services.AddOpenApi();

    builder.AddCORS();

    builder.Services.AddSingleton<IUserResolver, UserResolver>();

    builder.AddAuthenticationByJwtToken();


    builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(ConnectionStrings.SectionName));
    var connectionString = builder.Configuration[$"{ConnectionStrings.SectionName}:DefaultConnection"];
    var readonlyConnectionString = builder.Configuration[$"{ConnectionStrings.SectionName}:ReadOnlyConnection"];
    builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
        builder.AddModule(connectionString, readonlyConnectionString, typeof(ProductMapping).Assembly));

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
