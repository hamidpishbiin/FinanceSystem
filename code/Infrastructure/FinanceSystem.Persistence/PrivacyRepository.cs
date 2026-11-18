using FinanceSystem.Core;

namespace FinanceSystem.Persistence
{
    public class PrivacyRepository : IPrivacyRepository
    {
        public Task<UserPrivacy> GetBy(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
