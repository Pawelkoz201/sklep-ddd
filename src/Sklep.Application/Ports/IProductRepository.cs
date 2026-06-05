using Sklep.Domain.Products;

namespace Sklep.Application.Ports;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(Product product);
    void Remove(Product product);
}
