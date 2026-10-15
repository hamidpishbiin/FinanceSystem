namespace FinanceSystem.Domain.Users;

public interface IUserRepository : IRepository
{
    Task<bool> ExistsAsync(long id, CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken);
}
