using Sklep.Application.Common;
using Sklep.Application.DTOs;
using Sklep.Application.Ports;
using Sklep.Domain.Orders;
using Sklep.Domain.Products;

namespace Sklep.Application.Services;

public sealed class OrderService
{
    private readonly ICartRepository _carts;
    private readonly IOrderRepository _orders;
    private readonly IProductRepository _products;
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        ICartRepository carts,
        IOrderRepository orders,
        IProductRepository products,
        IUserRepository users,
        IUnitOfWork unitOfWork)
    {
        _carts = carts;
        _orders = orders;
        _products = products;
        _users = users;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<OrderResponse>> GetOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orders.GetAllAsync(cancellationToken);
        return orders.Select(MapOrder).ToList();
    }

    public async Task<OrderResponse> GetOrderAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Order not found.");

        return MapOrder(order);
    }

    public async Task<IReadOnlyList<OrderResponse>> GetOrdersByUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        await EnsureUserExistsAsync(userId, cancellationToken);
        var orders = await _orders.GetByUserIdAsync(userId, cancellationToken);
        return orders.Select(MapOrder).ToList();
    }

    public async Task<PlaceOrderResponse> PlaceOrderAsync(int userId, CancellationToken cancellationToken = default)
    {
        await EnsureUserExistsAsync(userId, cancellationToken);

        var cart = await _carts.GetByUserIdAsync(userId, cancellationToken)
            ?? throw new BusinessRuleViolationException("Cart is empty.");

        if (cart.Items.Count == 0)
        {
            throw new BusinessRuleViolationException("Cart is empty.");
        }

        var snapshots = new Dictionary<int, ProductSnapshot>();
        foreach (var item in cart.Items)
        {
            var product = await _products.GetByIdAsync(item.ProductId, cancellationToken)
                ?? throw new BusinessRuleViolationException($"Product with ID {item.ProductId} not found.");

            product.ReserveStock(item.Quantity);
            snapshots[product.Id] = product.Snapshot();
        }

        var order = Order.Place(userId, cart.Items, snapshots, DateTime.UtcNow);
        _orders.Add(order);
        cart.Clear();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new PlaceOrderResponse("Order placed successfully.", order.Id);
    }

    public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _orders.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Order not found.");

        _orders.Remove(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureUserExistsAsync(int userId, CancellationToken cancellationToken)
    {
        _ = await _users.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("User not found.");
    }

    private static OrderResponse MapOrder(Order order)
    {
        return new OrderResponse(
            order.Id,
            order.OrderDate,
            MapStatus(order.DeliveryStatus),
            order.TotalPrice.Amount,
            order.Items.Select(item => new OrderItemResponse(
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice.Amount,
                item.TotalPrice.Amount)).ToList());
    }

    private static string MapStatus(DeliveryStatus status)
    {
        return status switch
        {
            DeliveryStatus.InProgress => "W trakcie",
            DeliveryStatus.Sent => "Wyslane",
            DeliveryStatus.Delivered => "Dostarczone",
            DeliveryStatus.Cancelled => "Anulowane",
            _ => status.ToString()
        };
    }
}
