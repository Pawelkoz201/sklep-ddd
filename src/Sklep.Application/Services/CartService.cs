using Sklep.Application.Common;
using Sklep.Application.DTOs;
using Sklep.Application.Ports;
using Sklep.Domain.Carts;
using Sklep.Domain.Products;

namespace Sklep.Application.Services;

public sealed class CartService
{
    private readonly ICartRepository _carts;
    private readonly IProductRepository _products;
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;

    public CartService(
        ICartRepository carts,
        IProductRepository products,
        IUserRepository users,
        IUnitOfWork unitOfWork)
    {
        _carts = carts;
        _products = products;
        _users = users;
        _unitOfWork = unitOfWork;
    }

    public async Task AddToCartAsync(int userId, CartItemRequest request, CancellationToken cancellationToken = default)
    {
        await EnsureUserExistsAsync(userId, cancellationToken);
        var product = await GetProductOrThrowAsync(request.ProductId, cancellationToken);

        var cart = await _carts.GetByUserIdAsync(userId, cancellationToken);
        if (cart is null)
        {
            cart = new Cart(userId);
            _carts.Add(cart);
        }

        cart.AddItem(product.Id, request.Quantity, product.StockQuantity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<CartItemResponse> UpdateCartItemAsync(
        int userId,
        int productId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        await EnsureUserExistsAsync(userId, cancellationToken);
        var product = await GetProductOrThrowAsync(productId, cancellationToken);
        var cart = await GetCartOrThrowAsync(userId, cancellationToken);

        cart.ChangeQuantity(product.Id, quantity, product.StockQuantity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CartItemResponse(
            product.Id,
            product.Name,
            product.ImageUrl,
            product.Price.Amount,
            quantity,
            product.Price.Multiply(quantity).Amount);
    }

    public async Task RemoveCartItemAsync(int userId, int productId, CancellationToken cancellationToken = default)
    {
        await EnsureUserExistsAsync(userId, cancellationToken);
        var cart = await GetCartOrThrowAsync(userId, cancellationToken);

        cart.RemoveItem(productId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<CartResponse> GetCartAsync(int userId, CancellationToken cancellationToken = default)
    {
        await EnsureUserExistsAsync(userId, cancellationToken);
        var cart = await _carts.GetByUserIdAsync(userId, cancellationToken);

        if (cart is null || cart.Items.Count == 0)
        {
            return new CartResponse([], 0);
        }

        var snapshots = await GetProductSnapshotsAsync(cart.Items.Select(item => item.ProductId), cancellationToken);
        var items = cart.Items.Select(item =>
        {
            var product = snapshots[item.ProductId];
            return new CartItemResponse(
                product.ProductId,
                product.Name,
                product.ImageUrl,
                product.Price.Amount,
                item.Quantity,
                product.Price.Multiply(item.Quantity).Amount);
        }).ToList();

        return new CartResponse(items, cart.TotalValue(snapshots).Amount);
    }

    private async Task EnsureUserExistsAsync(int userId, CancellationToken cancellationToken)
    {
        _ = await _users.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("User not found.");
    }

    private async Task<Product> GetProductOrThrowAsync(int productId, CancellationToken cancellationToken)
    {
        return await _products.GetByIdAsync(productId, cancellationToken)
            ?? throw new NotFoundException("Product not found.");
    }

    private async Task<Cart> GetCartOrThrowAsync(int userId, CancellationToken cancellationToken)
    {
        return await _carts.GetByUserIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Cart not found.");
    }

    private async Task<IReadOnlyDictionary<int, ProductSnapshot>> GetProductSnapshotsAsync(
        IEnumerable<int> productIds,
        CancellationToken cancellationToken)
    {
        var snapshots = new Dictionary<int, ProductSnapshot>();
        foreach (var productId in productIds.Distinct())
        {
            var product = await GetProductOrThrowAsync(productId, cancellationToken);
            snapshots[product.Id] = product.Snapshot();
        }

        return snapshots;
    }
}
