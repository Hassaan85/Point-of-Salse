using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Repositories;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "ManagerOrAdmin")]
public sealed class ReportsController : ControllerBase
{
    private readonly IReportsRepository reportsRepository;

    public ReportsController(IReportsRepository reportsRepository)
    {
        this.reportsRepository = reportsRepository;
    }

    [HttpGet("sales-summary")]
    public async Task<IActionResult> SalesSummary([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
    {
        var rows = await reportsRepository.GetSalesSummaryAsync(from, to, ct);
        return Ok(rows);
    }

    [HttpGet("product-performance")]
    public async Task<IActionResult> ProductPerformance([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
    {
        var rows = await reportsRepository.GetProductPerformanceAsync(from, to, ct);
        return Ok(rows);
    }
}
