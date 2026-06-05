using Microsoft.AspNetCore.Mvc;
using Sklep.Application.DTOs;
using Sklep.Application.Services;

namespace Sklep.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CategoriesController : ControllerBase
{
    private readonly CatalogService _catalogService;

    public CategoriesController(CatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories(CancellationToken cancellationToken)
    {
        return Ok(await _catalogService.GetCategoriesAsync(cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryResponse>> GetCategory(int id, CancellationToken cancellationToken)
    {
        return Ok(await _catalogService.GetCategoryAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> PostCategory(
        [FromBody] CategoryRequest request,
        CancellationToken cancellationToken)
    {
        var category = await _catalogService.CreateCategoryAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutCategory(
        int id,
        [FromBody] CategoryRequest request,
        CancellationToken cancellationToken)
    {
        await _catalogService.UpdateCategoryAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
    {
        await _catalogService.DeleteCategoryAsync(id, cancellationToken);
        return NoContent();
    }
}
