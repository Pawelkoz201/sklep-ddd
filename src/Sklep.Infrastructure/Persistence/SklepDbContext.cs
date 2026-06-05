using Microsoft.EntityFrameworkCore;
using Sklep.Application.Ports;
using Sklep.Domain.Carts;
using Sklep.Domain.Customers;
using Sklep.Domain.Orders;
using Sklep.Domain.Products;
using Sklep.Domain.ValueObjects;

namespace Sklep.Infrastructure.Persistence;

public sealed class SklepDbContext : DbContext, IUnitOfWork
{
    public SklepDbContext(DbContextOptions<SklepDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    async Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUsers(modelBuilder);
        ConfigureCatalog(modelBuilder);
        ConfigureCarts(modelBuilder);
        ConfigureOrders(modelBuilder);
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable("Users");
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Username)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(user => user.Email)
                .HasConversion(email => email.Value, value => new EmailAddress(value))
                .HasColumnName("Email")
                .HasMaxLength(200)
                .IsRequired();
            builder.HasIndex(user => user.Email).IsUnique();
            builder.Property(user => user.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();
        });
    }

    private static void ConfigureCatalog(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(builder =>
        {
            builder.ToTable("Categories");
            builder.HasKey(category => category.Id);
            builder.Property(category => category.Name)
                .HasMaxLength(150)
                .IsRequired();
        });

        modelBuilder.Entity<Product>(builder =>
        {
            builder.ToTable("Products");
            builder.HasKey(product => product.Id);
            builder.Property(product => product.Name)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(product => product.ImageUrl)
                .HasMaxLength(500);
            builder.Property(product => product.Price)
                .HasConversion(price => price.Amount, value => new Money(value, "PLN"))
                .HasColumnName("Price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(product => product.StockQuantity).IsRequired();
            builder.Property(product => product.Description)
                .HasMaxLength(2000);
            builder.Property(product => product.CategoryId).IsRequired();
            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureCarts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(builder =>
        {
            builder.ToTable("Carts");
            builder.HasKey(cart => cart.Id);
            builder.Property(cart => cart.UserId).IsRequired();
            builder.HasIndex(cart => cart.UserId).IsUnique();
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(cart => cart.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(cart => cart.Items)
                .WithOne()
                .HasForeignKey("CartId")
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(cart => cart.Items)
                .HasField("_items")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<CartItem>(builder =>
        {
            builder.ToTable("CartItems");
            builder.HasKey(item => item.Id);
            builder.Property<int>("CartId");
            builder.Property(item => item.ProductId).IsRequired();
            builder.Property(item => item.Quantity).IsRequired();
        });
    }

    private static void ConfigureOrders(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(builder =>
        {
            builder.ToTable("Orders");
            builder.HasKey(order => order.Id);
            builder.Property(order => order.UserId).IsRequired();
            builder.Property(order => order.OrderDate).IsRequired();
            builder.Property(order => order.DeliveryStatus)
                .HasConversion<string>()
                .HasMaxLength(40)
                .IsRequired();
            builder.Ignore(order => order.TotalPrice);
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(order => order.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(order => order.Items)
                .WithOne()
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(order => order.Items)
                .HasField("_items")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<OrderItem>(builder =>
        {
            builder.ToTable("OrderItems");
            builder.HasKey(item => item.Id);
            builder.Property<int>("OrderId");
            builder.Property(item => item.ProductId).IsRequired();
            builder.Property(item => item.ProductName)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(item => item.ImageUrl)
                .HasMaxLength(500);
            builder.Property(item => item.UnitPrice)
                .HasConversion(price => price.Amount, value => new Money(value, "PLN"))
                .HasColumnName("Price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            builder.Property(item => item.Quantity).IsRequired();
            builder.Ignore(item => item.TotalPrice);
        });
    }
}
