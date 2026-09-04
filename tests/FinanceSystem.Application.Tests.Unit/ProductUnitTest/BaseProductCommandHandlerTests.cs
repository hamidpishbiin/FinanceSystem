using NSubstitute;
using FinanceSystem.Domain.Products;
using FinanceSystem.Domain.Tests.Unit.ProductsUnitTests;
using Shared.Core.EventHandlers;
using Shared.Core.Events;

namespace FinanceSystem.Application.Tests.Unit.ProductUnitTest
{
    public abstract class BaseProductCommandHandlerTests<T> where T : IDomainEvent
    {
        protected IProductRepository Repository;
        protected IEventPublisher EventPublisher;
        protected IEventListener EventListener;
        protected IEventHandler<T> EventHandler;

        private readonly ProductBuilder _builder;

        protected BaseProductCommandHandlerTests()
        {
            Repository = Substitute.For<IProductRepository>();
            EventPublisher = Substitute.For<IEventPublisher>();
            EventListener = Substitute.For<IEventListener>();
            EventHandler = Substitute.For<IEventHandler<T>>();
            _builder = new ProductBuilder();
        }

        protected async Task<Domain.Products.Product> CreateExpected()
        {
            var product = await _builder.WithName("Lubricant H250 z").WithDescription("descripton of Lubricant H250 z")
                .WithPrice(1000).WithStockQuantity(100).Build();
            return product;
        }
    }
}