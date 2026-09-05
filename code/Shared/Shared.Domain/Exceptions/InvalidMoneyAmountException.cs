using Shared.Core.Exceptions;

namespace Shared.Domain.Exceptions;

public class InvalidMoneyAmountException : BusinessException
{
    protected override int DefaultCode => BusinessExceptionCodes.InvalidMoneyAmount;
}
