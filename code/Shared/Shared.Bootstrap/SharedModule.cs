using Autofac;
using Shared.Application;
using Shared.Core;
using Shared.Core.Events;
using Shared.Serilog;
using Shared.EF;
using Shared.Autofac;

namespace Shared.Bootstrap
{
    public class SharedModule(string connectionString,string readonlyConnectionString, System.Reflection.Assembly[] mappingAssemblies) : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EntityIdBuilder<>))
                .As(typeof(IEntityIdBuilder<>))
                .SingleInstance();

            builder.RegisterGenericDecorator(typeof(TransactionalCommandHandlerDecorator<>), typeof(ICommandHandler<>));

            builder.RegisterType<CommandBus>().As<ICommandBus>()
                .InstancePerLifetimeScope();
            
            


            builder.RegisterType<SeriloggerService>().As<ILoggerService>()
                .SingleInstance();

            

            builder.RegisterType<EventAggregator>().As<IEventAggregator>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EventListener>().As<IEventListener>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EventPublisher>().As<IEventPublisher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SystemClock>().As<ISystemClock>()
                .SingleInstance();

            builder.RegisterType<GuidGenerator>().As<IGuidGenerator>()
                .SingleInstance();

            builder.RegisterType<UniqueIdGenerator>().As<IUniqueIdGenerator>()
                .SingleInstance();


            builder.RegisterServiceLocator();

            builder.RegisterDbContext(connectionString, readonlyConnectionString, mappingAssemblies);

        }
    }
}
