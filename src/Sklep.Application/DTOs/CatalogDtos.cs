namespace Sklep.Application.DTOs;

public sealed record CategoryResponse(int Id, string Name);

public sealed record CategoryRequest(string Name);

public sealed record ProductResponse(
    int Id,
    string Name,
    string ImageUrl,
    decimal Price,
    int StockQuantity,
    string Description,
    int CategoryId,
    CategoryResponse? Category);

public sealed record ProductRequest(
    string Name,
    string ImageUrl,
    decimal Price,
    int StockQuantity,
    string Description,
    int CategoryId);
