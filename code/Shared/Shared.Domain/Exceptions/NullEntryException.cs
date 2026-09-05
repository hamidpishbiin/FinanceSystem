using Shared.Core.Exceptions;

namespace Shared.Domain.Exceptions;

public class NullEntryException : BusinessException
{
    protected override int DefaultCode => BusinessExceptionCodes.NullEntry;
}
