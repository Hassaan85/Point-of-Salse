using Pos.Api.Models;

namespace Pos.Api.Repositories;

public interface ISalesRepository
{
    Task<CheckoutResponse> CheckoutAsync(CheckoutRequest request, CancellationToken ct);
}
