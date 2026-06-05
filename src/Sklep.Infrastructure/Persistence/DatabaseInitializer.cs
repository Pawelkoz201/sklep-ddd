using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sklep.Domain.Products;
using Sklep.Domain.ValueObjects;

namespace Sklep.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SklepDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
        await SeedCatalogAsync(dbContext);
    }

    private static async Task SeedCatalogAsync(SklepDbContext dbContext)
    {
        if (await dbContext.Categories.AnyAsync())
        {
            return;
        }

        var categories = new List<Category>
        {
            new("Rosliny doniczkowe"),
            new("Rosliny ogrodowe"),
            new("Sukulenty"),
            new("Ziola"),
            new("Kwiaty ciete"),
            new("Drzewka bonsai")
        };

        dbContext.Categories.AddRange(categories);
        await dbContext.SaveChangesAsync();

        var plantNames = new[]
        {
            "Monstera deliciosa", "Fikus benjamina", "Sansewieria", "Dracena marginata",
            "Zamiokulkas zamiolistny", "Aloes zwyczajny", "Kaktus opuncja", "Kalanchoe",
            "Bonsai fikus ginseng", "Rozmaryn", "Bazylia", "Lawenda", "Mieta pieprzowa",
            "Chryzantema", "Roza", "Tulipan", "Stokrotka", "Bluszcz pospolity", "Paprotka",
            "Anturium", "Orchidea", "Palma areka", "Juka", "Liwia", "Kroton",
            "Skrzydlokwiat", "Grubosz drzewiasty", "Eszeweria", "Haworcja", "Szalwia lekarska",
            "Tymianek", "Oregano", "Begonia", "Geranium", "Storczyk falenopsis",
            "Kaktus gwiazda betlejemska", "Hibiskus", "Azalia", "Magnolia", "Drzewko cytrynowe",
            "Drzewko oliwne", "Fiolek afrykanski", "Pelargonia", "Amarylis", "Asparagus",
            "Szeflera", "Papryczka chili", "Rozplenica japonska", "Kocanka wlochata"
        };

        var imageUrls = new[]
        {
            "http://localhost:5000/ProductImg/fikus.jpg",
            "http://localhost:5000/ProductImg/monstera.jpg",
            "http://localhost:5000/ProductImg/sansevieria.jpg"
        };

        var products = plantNames.Select((name, index) => new Product(
            name,
            imageUrls[index % imageUrls.Length],
            new Money(15 + (index * 5 % 185)),
            1 + (index * 3 % 50),
            $"Piekna roslina: {name}. Idealna do domu lub ogrodu.",
            categories[index % categories.Count].Id));

        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync();
    }
}
