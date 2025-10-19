using System.Data;
using Dapper;
using Pos.Api.Models;

namespace Pos.Api.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly IDbConnection connection;

    public ProductRepository(IDbConnection connection)
    {
        this.connection = connection;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(string? search, CancellationToken ct)
    {
        var results = await connection.QueryAsync<ProductDto>(
            new CommandDefinition(
                commandText: "dbo.spProducts_GetAll",
                parameters: new { Search = search },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            ));
        return results;
    }

    public async Task<ProductDto?> GetByIdAsync(int productId, CancellationToken ct)
    {
        return await connection.QuerySingleOrDefaultAsync<ProductDto>(
            new CommandDefinition(
                commandText: "dbo.spProducts_GetById",
                parameters: new { ProductId = productId },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            ));
    }

    public async Task<int> CreateAsync(ProductUpsertDto dto, CancellationToken ct)
    {
        var newId = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                commandText: "dbo.spProducts_Create",
                parameters: new
                {
                    dto.Name,
                    dto.SKU,
                    dto.Barcode,
                    dto.Price,
                    dto.TaxRate,
                    dto.ReorderThreshold
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            ));
        return newId;
    }

    public async Task<int> UpdateAsync(int productId, ProductUpsertDto dto, CancellationToken ct)
    {
        var updatedId = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                commandText: "dbo.spProducts_Update",
                parameters: new
                {
                    ProductId = productId,
                    dto.Name,
                    dto.SKU,
                    dto.Barcode,
                    dto.Price,
                    dto.TaxRate,
                    dto.ReorderThreshold,
                    dto.IsActive
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            ));
        return updatedId;
    }

    public async Task DeleteAsync(int productId, CancellationToken ct)
    {
        await connection.ExecuteAsync(
            new CommandDefinition(
                commandText: "dbo.spProducts_Delete",
                parameters: new { ProductId = productId },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            ));
    }
}
