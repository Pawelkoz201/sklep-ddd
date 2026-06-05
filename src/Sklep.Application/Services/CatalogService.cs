using Sklep.Application.Common;
using Sklep.Application.DTOs;
using Sklep.Application.Ports;
using Sklep.Domain.Products;
using Sklep.Domain.ValueObjects;

namespace Sklep.Application.Services;

public sealed class CatalogService
{
    private readonly ICategoryRepository _categories;
    private readonly IProductRepository _products;
    private readonly IUnitOfWork _unitOfWork;

    public CatalogService(
        ICategoryRepository categories,
        IProductRepository products,
        IUnitOfWork unitOfWork)
    {
        _categories = categories;
        _products = products;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ProductResponse>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _products.GetAllAsync(cancellationToken);
        return products.Select(MapProduct).ToList();
    }

    public async Task<IReadOnlyList<ProductResponse>> GetProductsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var products = await _products.GetByCategoryIdAsync(categoryId, cancellationToken);
        return products.Select(MapProduct).ToList();
    }

    public async Task<ProductResponse> GetProductAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);
        return MapProduct(product);
    }

    public async Task<ProductResponse> CreateProductAsync(ProductRequest request, CancellationToken cancellationToken = default)
    {
        await EnsureCategoryExistsAsync(request.CategoryId, cancellationToken);

        var product = new Product(
            request.Name,
            request.ImageUrl,
            new Money(request.Price),
            request.StockQuantity,
            request.Description,
            request.CategoryId);

        _products.Add(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapProduct(product);
    }

    public async Task UpdateProductAsync(int id, ProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);
        await EnsureCategoryExistsAsync(request.CategoryId, cancellationToken);

        product.Rename(request.Name);
        product.ChangeImage(request.ImageUrl);
        product.ChangePrice(new Money(request.Price));
        product.SetStock(request.StockQuantity);
        product.ChangeDescription(request.Description);
        product.MoveToCategory(request.CategoryId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);
        _products.Remove(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<CategoryResponse>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _categories.GetAllAsync(cancellationToken);
        return categories.Select(MapCategory).ToList();
    }

    public async Task<CategoryResponse> GetCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await GetCategoryOrThrowAsync(id, cancellationToken);
        return MapCategory(category);
    }

    public async Task<CategoryResponse> CreateCategoryAsync(CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = new Category(request.Name);
        _categories.Add(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapCategory(category);
    }

    public async Task UpdateCategoryAsync(int id, CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var category = await GetCategoryOrThrowAsync(id, cancellationToken);
        category.Rename(request.Name);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCategoryAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await GetCategoryOrThrowAsync(id, cancellationToken);
        _categories.Remove(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<Product> GetProductOrThrowAsync(int id, CancellationToken cancellationToken)
    {
        return await _products.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Product not found.");
    }

    private async Task<Category> GetCategoryOrThrowAsync(int id, CancellationToken cancellationToken)
    {
        return await _categories.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Category not found.");
    }

    private async Task EnsureCategoryExistsAsync(int categoryId, CancellationToken cancellationToken)
    {
        _ = await _categories.GetByIdAsync(categoryId, cancellationToken)
            ?? throw new BusinessRuleViolationException("Category does not exist.");
    }

    private static ProductResponse MapProduct(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.ImageUrl,
            product.Price.Amount,
            product.StockQuantity,
            product.Description,
            product.CategoryId,
            null);
    }

    private static CategoryResponse MapCategory(Category category)
    {
        return new CategoryResponse(category.Id, category.Name);
    }
}
