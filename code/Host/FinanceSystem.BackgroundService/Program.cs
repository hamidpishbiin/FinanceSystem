using Autofac;
using FinanceSystem.BackgroundService;
using FinanceSystem.Bootstrap;
using Serilog;
using Shared.Serilog;

const string appName = "FinanceSystem.BackgroundService";

Log.Logger = SerilogFactory.CreateLogger(appName);

var builder = WebApplication.CreateBuilder(args);

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

builder.Services
    .AddOptions<ConnectionStrings>()
    .Bind(builder.Configuration.GetSection(ConnectionStrings.SectionName));

builder.Host
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.AddBackgroundServiceModule(
            connectionStrings.DefaultConnection,
            connectionStrings.ReadOnlyConnection,
            BackgroundServiceAssemblyReference.Assembly);
    });

var app = builder.Build();

app.Run();

