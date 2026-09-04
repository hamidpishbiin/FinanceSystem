namespace FinanceSystem.Core
{
    public interface IPrivacyRepository
    {
        Task<UserPrivacy> GetBy(string userId);
    }
}
