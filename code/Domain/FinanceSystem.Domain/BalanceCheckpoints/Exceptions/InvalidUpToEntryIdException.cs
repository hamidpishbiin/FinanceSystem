namespace FinanceSystem.Domain.BalanceCheckpoints.Exceptions;

public class InvalidUpToEntryIdException : BusinessException
{
    protected override int DefaultCode => BalanceCheckpointExceptionCodes.InvalidUpToEntryId;
}
