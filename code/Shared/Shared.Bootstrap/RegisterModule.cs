using Autofac;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Shared.Bootstrap
{
    public static class RegisterModule
    {
        public static void AddFramework(this ContainerBuilder builder, string connectionString, string readonlyConnectionString, params Assembly[] mappingAssemblies)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>()
                .InstancePerLifetimeScope();

            builder.RegisterModule(new SharedModule(connectionString, readonlyConnectionString, mappingAssemblies));
        }
    }
}
