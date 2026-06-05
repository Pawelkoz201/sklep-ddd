using Microsoft.EntityFrameworkCore;
using Sklep.Application.Ports;
using Sklep.Domain.Carts;

namespace Sklep.Infrastructure.Persistence.Repositories;

public sealed class EfCartRepository : ICartRepository
{
    private readonly SklepDbContext _dbContext;

    public EfCartRepository(SklepDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Cart?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Carts
            .Include(cart => cart.Items)
            .FirstOrDefaultAsync(cart => cart.UserId == userId, cancellationToken);
    }

    public void Add(Cart cart)
    {
        _dbContext.Carts.Add(cart);
    }

    public void Remove(Cart cart)
    {
        _dbContext.Carts.Remove(cart);
    }
}
