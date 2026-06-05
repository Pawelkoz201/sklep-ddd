using Microsoft.EntityFrameworkCore;
using Sklep.Application.Ports;
using Sklep.Domain.Products;

namespace Sklep.Infrastructure.Persistence.Repositories;

public sealed class EfProductRepository : IProductRepository
{
    private readonly SklepDbContext _dbContext;

    public EfProductRepository(SklepDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .OrderBy(product => product.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .Where(product => product.CategoryId == categoryId)
            .OrderBy(product => product.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.FindAsync([id], cancellationToken);
    }

    public void Add(Product product)
    {
        _dbContext.Products.Add(product);
    }

    public void Remove(Product product)
    {
        _dbContext.Products.Remove(product);
    }
}
