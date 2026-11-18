using Autofac;
using Autofac.Extensions.DependencyInjection;
using FinanceSystem.BackgroundService;
using FinanceSystem.Bootstrap;
using FinanceSystem.Persistance;
using Serilog;
using Shared.Serilog;

const string appName = "FinanceSystem.BackgroundService";

Log.Logger = SerilogFactory.CreateLogger(appName);

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    builder.Services.AddHostedService<HeartbeatWorker>();

    var connectionStrings = builder.Configuration.GetSection(ConnectionStrings.SectionName).Get<ConnectionStrings>();

    if (connectionStrings == null)
    {
        throw new InvalidOperationException($"Configuration section '{ConnectionStrings.SectionName}' is missing.");
    }

    if (string.IsNullOrWhiteSpace(connectionStrings.DefaultConnection))
    {
        throw new InvalidOperationException($"{ConnectionStrings.SectionName}:DefaultConnection is required.");
    }

    if (string.IsNullOrWhiteSpace(connectionStrings.ReadOnlyConnection))
    {
        throw new InvalidOperationException($"{ConnectionStrings.SectionName}:ReadOnlyConnection is required.");
    }

    builder.Host
        .ConfigureContainer<ContainerBuilder>(builder =>
        {
            builder.AddBackgroundServiceModule(
                connectionStrings.DefaultConnection,
                connectionStrings.ReadOnlyConnection,
                PersistenceAssemblyReference.Assembly);
        });

    var app = builder.Build();

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

