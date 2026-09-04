using Autofac;
using Microsoft.EntityFrameworkCore;
using Shared.Core;
using System.Reflection;

namespace Shared.EF
{
    public static class ServiceRegistration
    {
        public static void RegisterDbContext(this ContainerBuilder builder,
            string connectionString,
            string readonlyConnectionString,
            Assembly mappingAssembly)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DBContext>().As<IDbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<QueryContext>().As<IQueryContext>()
                .InstancePerLifetimeScope();

            builder.Register(s =>
            {
                var options = new DbContextOptionsBuilder<EFContext>()
                    .UseNpgsql(connectionString);
                options.AddEFLoggingOptions();

                return new EFContext(options.Options, mappingAssembly);
            }).As<EFContext>().InstancePerLifetimeScope();

            builder.Register(s =>
            {
                var options = new DbContextOptionsBuilder<ReadOnlyContext>()
                    .UseNpgsql(connectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                
                options.AddEFLoggingOptions();

                return new ReadOnlyContext(options.Options, mappingAssembly);
            }).As<ReadOnlyContext>().InstancePerLifetimeScope();
        }

        private static void AddEFLoggingOptions(this DbContextOptionsBuilder options)
        {
            if (string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development", StringComparison.CurrentCultureIgnoreCase))
            {
                options.LogTo(Console.Write, Microsoft.Extensions.Logging.LogLevel.Information);
                options.EnableSensitiveDataLogging(true);
            }
        }
    }
}
