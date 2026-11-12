using Shared.Core.Exceptions;

namespace FinanceSystem.Domain.Contract.Exceptions;

public class InvalidEntityNameException : BusinessException
{
    protected override int DefaultCode => DomainEventExceptionCodes.InvalidEntityName;
}
