using Sklep.Domain.Products;

namespace Sklep.Application.Ports;

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(Category category);
    void Remove(Category category);
}
