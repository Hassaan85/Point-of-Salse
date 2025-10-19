using System.Data;
using System.Text.Json;
using Dapper;
using Pos.Api.Models;

namespace Pos.Api.Repositories;

public sealed class SalesRepository : ISalesRepository
{
    private readonly IDbConnection connection;

    public SalesRepository(IDbConnection connection)
    {
        this.connection = connection;
    }

    public async Task<CheckoutResponse> CheckoutAsync(CheckoutRequest request, CancellationToken ct)
    {
        var itemsJson = JsonSerializer.Serialize(request.Items);
        var paymentsJson = JsonSerializer.Serialize(request.Payments);
        return await connection.QuerySingleAsync<CheckoutResponse>(
            new CommandDefinition(
                commandText: "dbo.spSales_Checkout",
                parameters: new
                {
                    request.UserId,
                    request.CustomerId,
                    ItemsJson = itemsJson,
                    PaymentsJson = paymentsJson,
                    SaleDiscount = request.SaleDiscount
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            ));
    }
}
