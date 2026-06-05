using Microsoft.EntityFrameworkCore;
using Sklep.Application.Ports;
using Sklep.Domain.Customers;
using Sklep.Domain.ValueObjects;

namespace Sklep.Infrastructure.Persistence.Repositories;

public sealed class EfUserRepository : IUserRepository
{
    private readonly SklepDbContext _dbContext;

    public EfUserRepository(SklepDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .OrderBy(user => user.Username)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FindAsync([id], cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public void Add(User user)
    {
        _dbContext.Users.Add(user);
    }

    public void Remove(User user)
    {
        _dbContext.Users.Remove(user);
    }
}
