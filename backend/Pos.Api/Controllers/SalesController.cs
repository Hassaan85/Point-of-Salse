using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Models;
using Pos.Api.Repositories;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "CashierOrAbove")]
public sealed class SalesController : ControllerBase
{
    private readonly ISalesRepository salesRepository;

    public SalesController(ISalesRepository salesRepository)
    {
        this.salesRepository = salesRepository;
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutResponse>> Checkout([FromBody] CheckoutRequest request, CancellationToken ct)
    {
        var result = await salesRepository.CheckoutAsync(request, ct);
        return Ok(result);
    }
}
