using System.Reflection;
using Autofac;
using Shared.Bootstrap;

namespace FinanceSystem.Bootstrap;

public static class RegisterBackgroundServiceModule
{
    public static void AddBackgroundServiceModule(
        this ContainerBuilder builder,
        string connectionString,
        string readonlyConnectionString,
        params Assembly[] mappingAssemblies)
    {
        builder.AddFramework(connectionString, readonlyConnectionString, mappingAssemblies);

        builder.RegisterModule(new BackgroundServiceModule());
    }
}
