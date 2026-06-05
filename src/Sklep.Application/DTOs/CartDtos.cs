namespace Sklep.Application.DTOs;

public sealed record CartItemRequest(int ProductId, int Quantity);

public sealed record CartItemResponse(
    int ProductId,
    string Name,
    string ImageUrl,
    decimal Price,
    int Quantity,
    decimal TotalPrice);

public sealed record CartResponse(
    IReadOnlyCollection<CartItemResponse> CartItems,
    decimal TotalValue);
