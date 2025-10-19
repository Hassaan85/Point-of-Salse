namespace Pos.Api.Models;

public sealed class CheckoutItemDto
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal Discount { get; init; }
}

public sealed class PaymentDto
{
    public string Method { get; init; } = string.Empty;
    public decimal Amount { get; init; }
}

public sealed class CheckoutRequest
{
    public int UserId { get; init; }
    public int? CustomerId { get; init; }
    public decimal SaleDiscount { get; init; }
    public IEnumerable<CheckoutItemDto> Items { get; init; } = Array.Empty<CheckoutItemDto>();
    public IEnumerable<PaymentDto> Payments { get; init; } = Array.Empty<PaymentDto>();
}

public sealed class CheckoutResponse
{
    public int SaleId { get; init; }
    public decimal Subtotal { get; init; }
    public decimal Discount { get; init; }
    public decimal Tax { get; init; }
    public decimal Total { get; init; }
}
