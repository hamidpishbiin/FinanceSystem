using Shared.Core.Exceptions;

namespace FinanceSystem.Domain.Contract.Exceptions;

public class InvalidEventTypeException : BusinessException
{
    protected override int DefaultCode => DomainEventExceptionCodes.InvalidEventType;
}
