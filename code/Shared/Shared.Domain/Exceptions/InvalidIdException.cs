using Shared.Core.Exceptions;

namespace Shared.Domain.Exceptions;

public class InvalidIdException : BusinessException
{
    override protected int DefaultCode => BusinessExceptionCodes.InvalidId;
}
