namespace Sklep.Application.DTOs;

public sealed record OrderItemResponse(
    int ProductId,
    string Name,
    int Quantity,
    decimal Price,
    decimal TotalPrice);

public sealed record OrderResponse(
    int Id,
    DateTime OrderDate,
    string DeliveryStatus,
    decimal TotalPrice,
    IReadOnlyCollection<OrderItemResponse> Items);

public sealed record PlaceOrderResponse(string Message, int OrderId);
