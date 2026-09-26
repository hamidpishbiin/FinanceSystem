using FinanceSystem.Domain.Users;

namespace FinanceSystem.Persistance.Repositories;

public class UserRepository(IDbContext dbContext) : IUserRepository
{
    private DbSet<User> DbSet => dbContext.DbSet<User>();

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbSet.AnyAsync(user => user.Id == id, cancellationToken);
    }
}
