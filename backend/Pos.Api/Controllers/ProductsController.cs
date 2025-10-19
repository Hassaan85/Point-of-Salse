using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Models;
using Pos.Api.Repositories;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "ManagerOrAdmin")]
public sealed class ProductsController : ControllerBase
{
    private readonly IProductRepository productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    [HttpGet]
    [Authorize(Policy = "CashierOrAbove")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll([FromQuery] string? search, CancellationToken ct)
    {
        var results = await productRepository.GetAllAsync(search, ct);
        return Ok(results);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = "CashierOrAbove")]
    public async Task<ActionResult<ProductDto>> GetById(int id, CancellationToken ct)
    {
        var product = await productRepository.GetByIdAsync(id, ct);
        if (product is null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] ProductUpsertDto dto, CancellationToken ct)
    {
        var id = await productRepository.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> Update(int id, [FromBody] ProductUpsertDto dto, CancellationToken ct)
    {
        var updatedId = await productRepository.UpdateAsync(id, dto, ct);
        return Ok(updatedId);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await productRepository.DeleteAsync(id, ct);
        return NoContent();
    }
}
