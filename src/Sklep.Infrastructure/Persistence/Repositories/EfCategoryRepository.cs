using Microsoft.EntityFrameworkCore;
using Sklep.Application.Ports;
using Sklep.Domain.Products;

namespace Sklep.Infrastructure.Persistence.Repositories;

public sealed class EfCategoryRepository : ICategoryRepository
{
    private readonly SklepDbContext _dbContext;

    public EfCategoryRepository(SklepDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories
            .OrderBy(category => category.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Categories.FindAsync([id], cancellationToken);
    }

    public void Add(Category category)
    {
        _dbContext.Categories.Add(category);
    }

    public void Remove(Category category)
    {
        _dbContext.Categories.Remove(category);
    }
}
