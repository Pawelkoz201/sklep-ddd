using Sklep.Domain.Orders;

namespace Sklep.Application.Ports;

public interface IOrderRepository
{
    Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    void Add(Order order);
    void Remove(Order order);
}
