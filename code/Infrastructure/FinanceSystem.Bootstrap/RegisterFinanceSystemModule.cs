using Autofac;
using Shared.Bootstrap;
using System.Reflection;

namespace FinanceSystem.Bootstrap
{
    public static class RegisterFinanceSystemModule
    {
        public static void AddModule(this ContainerBuilder builder, 
            string connectionString, 
            string readonlyConnectionString,
            Assembly mappingAssembly)
        {
            builder.AddFramework(connectionString, readonlyConnectionString, mappingAssembly);
            builder.RegisterModule(new FinanceSystemModule());
        }
    }
}