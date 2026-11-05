namespace FinanceSystem.Domain.PspExchangeLogs.Exceptions;

public class InvalidDurationException : BusinessException
{
    protected override int DefaultCode => PspExchangeLogExceptionCodes.InvalidDuration;
}
