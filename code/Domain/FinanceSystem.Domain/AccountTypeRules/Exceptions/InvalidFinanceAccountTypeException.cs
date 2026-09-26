namespace FinanceSystem.Domain.AccountTypeRules.Exceptions;

public class InvalidFinanceAccountTypeException : BusinessException
{
    protected override int DefaultCode => AccountTypeRuleExceptionCodes.InvalidFinanceAccountType;
}
