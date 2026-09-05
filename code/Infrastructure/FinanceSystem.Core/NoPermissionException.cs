using Shared.Core.Exceptions;

namespace FinanceSystem.Core
{
    public static class SecurityExceptionCodes
    {
        public const int NoPermission = 900;
        public const int AccessDenied = 901;
    }

    public class NoPermissionException : BusinessException
    {
        protected override int DefaultCode => SecurityExceptionCodes.NoPermission;
    }

    public class AccessDeniedException : BusinessException
    {
        protected override int DefaultCode => SecurityExceptionCodes.AccessDenied;
    }
}
