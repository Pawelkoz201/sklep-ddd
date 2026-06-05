using Microsoft.EntityFrameworkCore;
using Sklep.Application.Ports;
using Sklep.Domain.Orders;

namespace Sklep.Infrastructure.Persistence.Repositories;

public sealed class EfOrderRepository : IOrderRepository
{
    private readonly SklepDbContext _dbContext;

    public EfOrderRepository(SklepDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(order => order.Items)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(order => order.Items)
            .Where(order => order.UserId == userId)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public void Add(Order order)
    {
        _dbContext.Orders.Add(order);
    }

    public void Remove(Order order)
    {
        _dbContext.Orders.Remove(order);
    }
}
