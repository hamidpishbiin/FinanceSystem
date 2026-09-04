using Autofac;
using Autofac.Extras.DynamicProxy;
using FinanceSystem.Application.Product.CommandHandlers;
using FinanceSystem.Core;
using FinanceSystem.Domain.EventHandlers.ProductEventHandlers;
using FinanceSystem.Domain.Products.DomainServices;
using FinanceSystem.Interface.ReadModel;
using FinanceSystem.Interface.WriteModel;
using FinanceSystem.Persistance;
using FinanceSystem.Persistance.Repositories;
using Shared.Application;
using Shared.Core;
using Shared.Core.EventHandlers;
using Shared.Domain;

namespace FinanceSystem.Bootstrap
{
    public class FinanceSystemModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ProductRepository).Assembly)
                .Where(a => typeof(IRepository).IsAssignableFrom(a))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<PrivacyRepository>().As<IPrivacyRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType(typeof(SecurityInterceptor));

            builder.RegisterAssemblyTypes(typeof(ProductFacadeService).Assembly)
                .Where(a => typeof(IFacadeService).IsAssignableFrom(a))
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(SecurityInterceptor))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ProductFacadeQuery).Assembly)
                .Where(a => typeof(IFacadeService).IsAssignableFrom(a))
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(SecurityInterceptor))
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(StockQuantityService).Assembly)
                .Where(a => typeof(IDomainService).IsAssignableFrom(a))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ProductCreatedEventHandler).Assembly)
                .AsClosedTypesOf(typeof(IEventHandler<>)).InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(typeof(ProductCreateCommandHandler).Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerLifetimeScope();
        }
    }
}