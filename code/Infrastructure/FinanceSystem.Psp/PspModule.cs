using Autofac;
using FinanceSystem.Application.Payments.Gateways;

namespace FinanceSystem.Psp;

public class PspModule : Module
{
    override protected void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(ThisAssembly)
            .AssignableTo<IPspGateway>()
            .As<IPspGateway>()
            .InstancePerLifetimeScope();

        builder
            .RegisterType<PspGatewayFactory>()
            .As<IPspGatewayFactory>()
            .InstancePerLifetimeScope();
    }
}
