using Shared.Core;

namespace FinanceSystem.Presentation
{
    public class UserResolver : IUserResolver
    {
        private readonly List<Tuple<string, string>> _claims;

        public UserResolver(IHttpContextAccessor httpContextAccessor)
        {
            _claims = httpContextAccessor.HttpContext.User.Claims
                .Select(a => new Tuple<string, string>(a.Type, a.Value))
                .ToList();
        }

        public string GetUserId() => _claims?.FirstOrDefault(a => a.Item1.Split('/').Last() == "nameidentifier")?.Item2 ?? throw new UnauthorizedAccessException();
        public string GetUsername() => _claims?.FirstOrDefault(a => a.Item1 == "username")?.Item2 ?? "^-^";
    }
}
