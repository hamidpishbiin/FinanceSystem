using FinanceSystem.Domain.Users;

namespace FinanceSystem.Persistance.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly DbSet<User> _dbSet;

    public UserRepository(IDbContext context)
    {
        _dbSet = context.DbSet<User>();
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken)
    {
        return await _dbSet.AnyAsync(user => user.Id == id, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _dbSet.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }
}
