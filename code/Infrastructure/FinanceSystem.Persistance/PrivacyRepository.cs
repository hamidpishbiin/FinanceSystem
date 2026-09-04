using FinanceSystem.Core;

namespace FinanceSystem.Persistance
{
    public class PrivacyRepository : IPrivacyRepository
    {
        public Task<UserPrivacy> GetBy(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
