using Microsoft.AspNetCore.Mvc;
using Sklep.Application.DTOs;
using Sklep.Application.Services;

namespace Sklep.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders(CancellationToken cancellationToken)
    {
        return Ok(await _orderService.GetOrdersAsync(cancellationToken));
    }

    [HttpPost("{userId:int}/placeorder")]
    public async Task<ActionResult<PlaceOrderResponse>> PlaceOrder(int userId, CancellationToken cancellationToken)
    {
        return Ok(await _orderService.PlaceOrderAsync(userId, cancellationToken));
    }

    [HttpGet("{userId:int}/orders")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUser(
        int userId,
        CancellationToken cancellationToken)
    {
        return Ok(await _orderService.GetOrdersByUserAsync(userId, cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(int id, CancellationToken cancellationToken)
    {
        return Ok(await _orderService.GetOrderAsync(id, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id, CancellationToken cancellationToken)
    {
        await _orderService.DeleteOrderAsync(id, cancellationToken);
        return NoContent();
    }
}
