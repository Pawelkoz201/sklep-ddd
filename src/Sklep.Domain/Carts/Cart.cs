using Sklep.Domain.Common;
using Sklep.Domain.Products;
using Sklep.Domain.ValueObjects;

namespace Sklep.Domain.Carts;

public sealed class Cart : Entity
{
    private readonly List<CartItem> _items = [];

    private Cart()
    {
    }

    public Cart(int userId)
    {
        if (userId <= 0)
        {
            throw new DomainException("Cart user id must be positive.");
        }

        UserId = userId;
    }

    public int UserId { get; private set; }
    public IReadOnlyCollection<CartItem> Items => _items;

    public void AddItem(int productId, int quantity, int availableStock)
    {
        if (productId <= 0)
        {
            throw new DomainException("Product id must be positive.");
        }

        var existingItem = _items.FirstOrDefault(item => item.ProductId == productId);
        if (existingItem is null)
        {
            EnsureRequestedStockIsAvailable(quantity, availableStock);
            _items.Add(new CartItem(productId, quantity));
            return;
        }

        existingItem.IncreaseQuantity(quantity, availableStock);
    }

    public void ChangeQuantity(int productId, int quantity, int availableStock)
    {
        if (productId <= 0)
        {
            throw new DomainException("Product id must be positive.");
        }

        var item = FindItem(productId);
        item.ChangeQuantity(quantity, availableStock);
    }

    public void RemoveItem(int productId)
    {
        var item = FindItem(productId);
        _items.Remove(item);
    }

    public void Clear()
    {
        _items.Clear();
    }

    public Money TotalValue(IReadOnlyDictionary<int, ProductSnapshot> products)
    {
        var total = Money.Zero();
        foreach (var item in _items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
            {
                throw new DomainException($"Product {item.ProductId} is missing from cart calculation.");
            }

            total = total.Add(product.Price.Multiply(item.Quantity));
        }

        return total;
    }

    private CartItem FindItem(int productId)
    {
        return _items.FirstOrDefault(item => item.ProductId == productId)
            ?? throw new DomainException("Product not found in cart.");
    }

    private static void EnsureRequestedStockIsAvailable(int quantity, int availableStock)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be positive.");
        }

        if (availableStock < quantity)
        {
            throw new DomainException("Insufficient stock.");
        }
    }
}

public sealed class CartItem : Entity
{
    private CartItem()
    {
    }

    internal CartItem(int productId, int quantity)
    {
        if (productId <= 0)
        {
            throw new DomainException("Cart item product id must be positive.");
        }

        ProductId = productId;
        ChangeQuantity(quantity, int.MaxValue);
    }

    public int ProductId { get; private set; }
    public int Quantity { get; private set; }

    internal void IncreaseQuantity(int quantity, int availableStock)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be positive.");
        }

        ChangeQuantity(Quantity + quantity, availableStock);
    }

    internal void ChangeQuantity(int quantity, int availableStock)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Quantity must be positive.");
        }

        if (availableStock < quantity)
        {
            throw new DomainException("Insufficient stock.");
        }

        Quantity = quantity;
    }
}
