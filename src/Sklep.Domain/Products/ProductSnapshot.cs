using Sklep.Domain.ValueObjects;

namespace Sklep.Domain.Products;

public sealed record ProductSnapshot(int ProductId, string Name, string ImageUrl, Money Price);
