using Sklep.Domain.Common;

namespace Sklep.Domain.Products;

public sealed class Category : Entity
{
    private Category()
    {
        Name = string.Empty;
    }

    public Category(string name)
    {
        Rename(name);
    }

    public string Name { get; private set; } = string.Empty;

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Category name is required.");
        }

        Name = name.Trim();
    }
}
