namespace FinanceSystem.Domain.AccountTypeRules.Exceptions;

public class InvalidAccountTypeException : BusinessException
{
    protected override int DefaultCode => AccountTypeRuleExceptionCodes.InvalidAccountType;
}
