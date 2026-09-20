using Shared.Core;

namespace Shared.Application
{
    public interface ICommandHandler
    {

    }
    public interface ICommandHandler<in T> : ICommandHandler where T : ICommand
    {
        Task Handle(T command, CancellationToken cancellationToken = default);
    }
}
