using Microsoft.AspNetCore.Mvc;
using Sklep.Application.DTOs;
using Sklep.Application.Services;

namespace Sklep.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ProductsController : ControllerBase
{
    private readonly CatalogService _catalogService;

    public ProductsController(CatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts(CancellationToken cancellationToken)
    {
        return Ok(await _catalogService.GetProductsAsync(cancellationToken));
    }

    [HttpGet("Category/{categoryId:int}")]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByCategory(
        int categoryId,
        CancellationToken cancellationToken)
    {
        var products = await _catalogService.GetProductsByCategoryAsync(categoryId, cancellationToken);
        if (products.Count == 0)
        {
            return NotFound($"No products found for CategoryId {categoryId}");
        }

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetProduct(int id, CancellationToken cancellationToken)
    {
        return Ok(await _catalogService.GetProductAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> PostProduct(
        [FromBody] ProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _catalogService.CreateProductAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutProduct(
        int id,
        [FromBody] ProductRequest request,
        CancellationToken cancellationToken)
    {
        await _catalogService.UpdateProductAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
    {
        await _catalogService.DeleteProductAsync(id, cancellationToken);
        return NoContent();
    }
}
