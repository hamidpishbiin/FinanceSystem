using Shared.Core;

namespace Shared.Application
{
    public class CommandBus(IServiceLocator locator) : ICommandBus
    {
        public async Task Dispatch<T>(T command, CancellationToken cancellationToken = default) where T : ICommand
        {
            var execute = locator.GetInstance<ICommandHandler<T>>();
            await execute.Handle(command, cancellationToken);
        }
    }
}
