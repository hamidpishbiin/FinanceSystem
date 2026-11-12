using Shared.Core.Exceptions;

namespace FinanceSystem.Domain.Contract.Exceptions;

public class InvalidEventStateException : BusinessException
{
    protected override int DefaultCode => DomainEventExceptionCodes.InvalidEventState;
}
