using Sklep.Domain.Carts;
using Sklep.Domain.Common;
using Sklep.Domain.Products;
using Sklep.Domain.ValueObjects;

namespace Sklep.Domain.Orders;

public sealed class Order : Entity
{
    private readonly List<OrderItem> _items = [];

    private Order()
    {
    }

    private Order(int userId, DateTime orderDate)
    {
        if (userId <= 0)
        {
            throw new DomainException("Order user id must be positive.");
        }

        UserId = userId;
        OrderDate = orderDate;
        DeliveryStatus = DeliveryStatus.InProgress;
    }

    public int UserId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public DeliveryStatus DeliveryStatus { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items;
    public Money TotalPrice => _items.Aggregate(Money.Zero(), (sum, item) => sum.Add(item.TotalPrice));

    public static Order Place(
        int userId,
        IEnumerable<CartItem> cartItems,
        IReadOnlyDictionary<int, ProductSnapshot> products,
        DateTime orderDate)
    {
        var items = cartItems.ToList();
        if (items.Count == 0)
        {
            throw new DomainException("Cart is empty.");
        }

        var order = new Order(userId, orderDate);
        foreach (var cartItem in items)
        {
            if (!products.TryGetValue(cartItem.ProductId, out var product))
            {
                throw new DomainException($"Product {cartItem.ProductId} is missing from order.");
            }

            order._items.Add(new OrderItem(product, cartItem.Quantity));
        }

        return order;
    }

    public void MarkAsSent()
    {
        if (DeliveryStatus != DeliveryStatus.InProgress)
        {
            throw new DomainException("Only in-progress orders can be sent.");
        }

        DeliveryStatus = DeliveryStatus.Sent;
    }

    public void MarkAsDelivered()
    {
        if (DeliveryStatus == DeliveryStatus.Cancelled)
        {
            throw new DomainException("Cancelled order cannot be delivered.");
        }

        DeliveryStatus = DeliveryStatus.Delivered;
    }

    public void Cancel()
    {
        if (DeliveryStatus == DeliveryStatus.Delivered)
        {
            throw new DomainException("Delivered order cannot be cancelled.");
        }

        DeliveryStatus = DeliveryStatus.Cancelled;
    }
}

public sealed class OrderItem : Entity
{
    private OrderItem()
    {
        ProductName = string.Empty;
        ImageUrl = string.Empty;
        UnitPrice = Money.Zero();
    }

    internal OrderItem(ProductSnapshot product, int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("Order item quantity must be positive.");
        }

        ProductId = product.ProductId;
        ProductName = product.Name;
        ImageUrl = product.ImageUrl;
        UnitPrice = product.Price;
        Quantity = quantity;
    }

    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string ImageUrl { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public Money TotalPrice => UnitPrice.Multiply(Quantity);
}
