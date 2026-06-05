using Microsoft.AspNetCore.Mvc;
using Sklep.Application.DTOs;
using Sklep.Application.Services;

namespace Sklep.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class UsersController : ControllerBase
{
    private readonly CartService _cartService;
    private readonly UserService _userService;

    public UsersController(UserService userService, CartService cartService)
    {
        _userService = userService;
        _cartService = cartService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = await _userService.RegisterAsync(request, cancellationToken);
        return Ok(new { Message = "Registration successful", User = user });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _userService.LoginAsync(request, cancellationToken));
    }

    [HttpPost("{userId:int}/addToCart")]
    public async Task<IActionResult> AddToCart(
        int userId,
        [FromBody] CartItemRequest request,
        CancellationToken cancellationToken)
    {
        await _cartService.AddToCartAsync(userId, request, cancellationToken);
        return Ok("Item added to cart.");
    }

    [HttpPut("{userId:int}/cart/{productId:int}")]
    public async Task<ActionResult<CartItemResponse>> UpdateCartItem(
        int userId,
        int productId,
        [FromBody] int quantity,
        CancellationToken cancellationToken)
    {
        return Ok(await _cartService.UpdateCartItemAsync(userId, productId, quantity, cancellationToken));
    }

    [HttpDelete("{userId:int}/cart/{productId:int}")]
    public async Task<IActionResult> RemoveCartItem(int userId, int productId, CancellationToken cancellationToken)
    {
        await _cartService.RemoveCartItemAsync(userId, productId, cancellationToken);
        return Ok(new { Message = "Item removed from cart." });
    }

    [HttpGet("{userId:int}/cart")]
    public async Task<ActionResult<CartResponse>> GetCart(int userId, CancellationToken cancellationToken)
    {
        return Ok(await _cartService.GetCartAsync(userId, cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers(CancellationToken cancellationToken)
    {
        return Ok(await _userService.GetUsersAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetUser(int id, CancellationToken cancellationToken)
    {
        return Ok(await _userService.GetUserAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> PostUser(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.RegisterAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutUser(
        int id,
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        await _userService.UpdateUserAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        await _userService.DeleteUserAsync(id, cancellationToken);
        return NoContent();
    }
}
