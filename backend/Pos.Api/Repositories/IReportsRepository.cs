namespace Pos.Api.Repositories;

public interface IReportsRepository
{
    Task<IEnumerable<SalesSummaryRow>> GetSalesSummaryAsync(DateTime from, DateTime to, CancellationToken ct);
    Task<IEnumerable<ProductPerformanceRow>> GetProductPerformanceAsync(DateTime from, DateTime to, CancellationToken ct);
}

public sealed class SalesSummaryRow
{
    public DateTime Date { get; init; }
    public int NumSales { get; init; }
    public decimal Subtotal { get; init; }
    public decimal Discount { get; init; }
    public decimal Tax { get; init; }
    public decimal Total { get; init; }
}

public sealed class ProductPerformanceRow
{
    public int ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int QuantitySold { get; init; }
    public decimal Revenue { get; init; }
}
