using Sklep.Domain.Carts;

namespace Sklep.Application.Ports;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    void Add(Cart cart);
    void Remove(Cart cart);
}
