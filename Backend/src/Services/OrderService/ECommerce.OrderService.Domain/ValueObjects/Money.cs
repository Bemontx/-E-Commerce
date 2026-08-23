namespace ECommerce.OrderService.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException(
                "Money amount cannot be negative.",
                nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException(
                "Currency is required.",
                nameof(currency));

        Currency = currency.ToUpperInvariant();
        Amount = amount;
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);

        return new Money(
            Amount + other.Amount,
            Currency);
    }

    public Money Multiply(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException(
                "Quantity cannot be negative.",
                nameof(quantity));

        return new Money(
            Amount * quantity,
            Currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException(
                "Cannot operate with different currencies.");
    }
}