using Shared.Core.Exceptions;

namespace FinanceSystem.Domain.Contract.Exceptions;

public class InvalidEventIdException : BusinessException
{
    protected override int DefaultCode => DomainEventExceptionCodes.InvalidEventId;
}
