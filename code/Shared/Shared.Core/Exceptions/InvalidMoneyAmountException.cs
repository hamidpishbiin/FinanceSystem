namespace Shared.Core.Exceptions;

public class InvalidMoneyAmountException : BusinessException
{
    protected override int DefaultCode => BusinessExceptionCodes.InvalidMoneyAmount;
}
