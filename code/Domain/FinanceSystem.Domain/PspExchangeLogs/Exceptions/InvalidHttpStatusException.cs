namespace FinanceSystem.Domain.PspExchangeLogs.Exceptions;

public class InvalidHttpStatusException : BusinessException
{
    protected override int DefaultCode => PspExchangeLogExceptionCodes.InvalidHttpStatus;
}
