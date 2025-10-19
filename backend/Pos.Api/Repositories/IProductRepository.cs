using Pos.Api.Models;

namespace Pos.Api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetAllAsync(string? search, CancellationToken ct);
    Task<ProductDto?> GetByIdAsync(int productId, CancellationToken ct);
    Task<int> CreateAsync(ProductUpsertDto dto, CancellationToken ct);
    Task<int> UpdateAsync(int productId, ProductUpsertDto dto, CancellationToken ct);
    Task DeleteAsync(int productId, CancellationToken ct);
}
