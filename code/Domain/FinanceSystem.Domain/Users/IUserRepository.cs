namespace FinanceSystem.Domain.Users;

public interface IUserRepository : IRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}
