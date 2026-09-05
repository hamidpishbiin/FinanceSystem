namespace Shared.Core.Exceptions;

public class InvalidMoneyCurrencyException : BusinessException
{
    protected override int DefaultCode => BusinessExceptionCodes.InvalidMoneyCurrency;
}
