using Sklep.Domain.Common;
using Sklep.Domain.ValueObjects;

namespace Sklep.Domain.Products;

public sealed class Product : Entity
{
    private Product()
    {
        Name = string.Empty;
        ImageUrl = string.Empty;
        Description = string.Empty;
        Price = Money.Zero();
    }

    public Product(
        string name,
        string imageUrl,
        Money price,
        int stockQuantity,
        string description,
        int categoryId)
    {
        Rename(name);
        ChangeImage(imageUrl);
        ChangePrice(price);
        SetStock(stockQuantity);
        ChangeDescription(description);
        MoveToCategory(categoryId);
    }

    public string Name { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;
    public Money Price { get; private set; } = Money.Zero();
    public int StockQuantity { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public int CategoryId { get; private set; }

    public ProductSnapshot Snapshot()
    {
        return new ProductSnapshot(Id, Name, ImageUrl, Price);
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Product name is required.");
        }

        Name = name.Trim();
    }

    public void ChangeImage(string imageUrl)
    {
        ImageUrl = imageUrl?.Trim() ?? string.Empty;
    }

    public void ChangeDescription(string description)
    {
        Description = description?.Trim() ?? string.Empty;
    }

    public void ChangePrice(Money price)
    {
        Price = price ?? throw new DomainException("Product price is required.");
    }

    public void MoveToCategory(int categoryId)
    {
        if (categoryId <= 0)
        {
            throw new DomainException("Product category id must be positive.");
        }

        CategoryId = categoryId;
    }

    public void SetStock(int quantity)
    {
        if (quantity < 0)
        {
            throw new DomainException("Stock quantity cannot be negative.");
        }

        StockQuantity = quantity;
    }

    public void ReserveStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Reserved quantity must be positive.");
        }

        if (StockQuantity < quantity)
        {
            throw new DomainException($"Insufficient stock for product: {Name}.");
        }

        StockQuantity -= quantity;
    }
}
