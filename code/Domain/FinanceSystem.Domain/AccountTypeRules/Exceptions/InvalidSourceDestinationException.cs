namespace FinanceSystem.Domain.AccountTypeRules.Exceptions;

public class InvalidSourceDestinationException : BusinessException
{
    override protected int DefaultCode => AccountTypeRuleExceptionCodes.InvalidSourceDestination;
}
