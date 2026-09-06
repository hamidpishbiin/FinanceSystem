namespace FinanceSystem.Domain.BalanceCheckpoints.Exceptions;

public class InvalidAccountIdException : BusinessException
{
    protected override int DefaultCode => BalanceCheckpointExceptionCodes.InvalidAccountId;
}
