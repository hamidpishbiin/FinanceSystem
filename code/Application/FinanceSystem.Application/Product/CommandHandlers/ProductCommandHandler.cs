global using Shared.Application;
global using Shared.Core.EventHandlers;
global using Shared.Core.Events;
global using Shared.Core.Exceptions;
global using Shared.Core;

using FinanceSystem.Domain.Products;

namespace FinanceSystem.Application.Product.CommandHandlers
{
    public abstract class ProductCommandHandler<T, TEvent> : BaseCommandHandler<T, TEvent> where TEvent : IDomainEvent where T : ICommand
    {
        protected readonly IProductRepository Repository;

        protected ProductCommandHandler(IProductRepository repository, IEventPublisher publisher, IEventListener listener, IEventHandler<TEvent> eventHandler)
            : base(publisher, listener, eventHandler)
        {
            Repository = repository;
        }
    }
}
