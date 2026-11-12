using Shared.Core.Exceptions;

namespace FinanceSystem.Domain.Contract.Exceptions;

public class InvalidHappenDateTimeException : BusinessException
{
    protected override int DefaultCode => DomainEventExceptionCodes.InvalidHappenDateTime;
}
