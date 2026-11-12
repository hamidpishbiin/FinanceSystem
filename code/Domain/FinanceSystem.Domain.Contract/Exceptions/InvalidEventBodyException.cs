using Shared.Core.Exceptions;

namespace FinanceSystem.Domain.Contract.Exceptions;

public class InvalidEventBodyException : BusinessException
{
    protected override int DefaultCode => DomainEventExceptionCodes.InvalidEventBody;
}
