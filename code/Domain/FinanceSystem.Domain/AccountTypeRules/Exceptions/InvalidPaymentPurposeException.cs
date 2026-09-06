namespace FinanceSystem.Domain.AccountTypeRules.Exceptions;

public class InvalidPaymentPurposeException : BusinessException
{
    protected override int DefaultCode => AccountTypeRuleExceptionCodes.InvalidPaymentPurpose;
}
