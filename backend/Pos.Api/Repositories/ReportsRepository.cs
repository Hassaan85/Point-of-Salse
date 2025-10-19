using System.Data;
using Dapper;

namespace Pos.Api.Repositories;

public sealed class ReportsRepository : IReportsRepository
{
    private readonly IDbConnection connection;

    public ReportsRepository(IDbConnection connection)
    {
        this.connection = connection;
    }

    public async Task<IEnumerable<SalesSummaryRow>> GetSalesSummaryAsync(DateTime from, DateTime to, CancellationToken ct)
    {
        return await connection.QueryAsync<SalesSummaryRow>(
            new CommandDefinition(
                commandText: "dbo.spReports_SalesSummary",
                parameters: new { From = from, To = to },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            ));
    }

    public async Task<IEnumerable<ProductPerformanceRow>> GetProductPerformanceAsync(DateTime from, DateTime to, CancellationToken ct)
    {
        return await connection.QueryAsync<ProductPerformanceRow>(
            new CommandDefinition(
                commandText: "dbo.spReports_ProductPerformance",
                parameters: new { From = from, To = to },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            ));
    }
}
