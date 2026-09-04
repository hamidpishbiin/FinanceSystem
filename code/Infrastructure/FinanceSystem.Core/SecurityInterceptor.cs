using Castle.DynamicProxy;
using Shared.Core;
using Shared.Core.Exceptions;
using System.Reflection;

namespace FinanceSystem.Core
{
    public class SecurityInterceptor : IInterceptor
    {
        private readonly IUserResolver _userResolver;
        private readonly IPrivacyRepository _repository;

        public SecurityInterceptor(IUserResolver userResolver, IPrivacyRepository repository)
        {
            _userResolver = userResolver;
            _repository = repository;
        }

        public void Intercept(IInvocation invocation)
        {
            //var permission = GetPermissionFromMethod(invocation);
            //if (permission != null)
            //    CheckIfUserHasPermission(permission).Wait();
            //else
            //    CheckIfNoPermissionNeeded(invocation);
            invocation.Proceed();
        }

        private static HasPermissionAttribute GetPermissionFromMethod(IInvocation invocation)
        {
            return invocation.Method.GetCustomAttributes<HasPermissionAttribute>(true).FirstOrDefault();
        }

        private async Task CheckIfUserHasPermission(HasPermissionAttribute permission)
        {
            var user = await _repository.GetBy(_userResolver.GetUserId());
            Guard<AccessDeniedException>.AgainstNull(user);
            Guard<AccessDeniedException>.AgainstNull(user.AllowPermissions);
            Guard<NoPermissionException>.SmallerThan(user.AllowPermissions.Count, 1);

            var allowedByPermission = user.AllowPermissions.Any(z => z == permission.PermissionCode);

            var allowedByRole = false;
            if (permission.Roles.Length > 0)
                allowedByRole = user.RolesList.Any(a => permission.Roles.Any(z => z == a));

            if (!allowedByPermission && !allowedByRole)
                throw new AccessDeniedException();
        }

        private static void CheckIfNoPermissionNeeded(IInvocation invocation)
        {
            var ignorePermission = invocation.Method.GetCustomAttributes<IgnorePermissionAttribute>(true).FirstOrDefault();
            Guard<NoPermissionException>.AgainstNull(ignorePermission);
        }
    }
}
