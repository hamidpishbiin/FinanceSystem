using Shared.Core.Exceptions;

namespace Shared.Domain.Exceptions;

public class InvalidMoneyCurrencyException : BusinessException
{
    protected override int DefaultCode => BusinessExceptionCodes.InvalidMoneyCurrency;
}
