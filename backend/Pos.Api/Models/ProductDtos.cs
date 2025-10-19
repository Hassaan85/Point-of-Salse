namespace Pos.Api.Models;

public sealed class ProductDto
{
    public int ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string SKU { get; init; } = string.Empty;
    public string? Barcode { get; init; }
    public decimal Price { get; init; }
    public decimal TaxRate { get; init; }
    public int ReorderThreshold { get; init; }
    public bool IsActive { get; init; }
    public int Quantity { get; init; }
}

public sealed class ProductUpsertDto
{
    public string Name { get; init; } = string.Empty;
    public string SKU { get; init; } = string.Empty;
    public string? Barcode { get; init; }
    public decimal Price { get; init; }
    public decimal TaxRate { get; init; }
    public int ReorderThreshold { get; init; }
    public bool IsActive { get; init; } = true;
}
