namespace FinanceSystem.Domain.BalanceCheckpoints.Exceptions;

public class InvalidFinanceAccountIdException : BusinessException
{
    protected override int DefaultCode => BalanceCheckpointExceptionCodes.InvalidFinanceAccountId;
}
