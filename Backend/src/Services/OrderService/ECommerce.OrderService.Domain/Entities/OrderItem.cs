using ECommerce.OrderService.Domain.ValueObjects;

namespace ECommerce.OrderService.Domain.Entities;

public sealed class OrderItem
{
    public Guid Id { get; private set; }

    public Guid ProductId { get; private set; }

    public int Quantity { get; private set; }

    public Money UnitPrice { get; private set; }

    public Money Total =>
        UnitPrice.Multiply(Quantity);

    private OrderItem()
    {
    }

    public OrderItem(
        Guid productId,
        int quantity,
        Money unitPrice)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException(
                "ProductId is required.",
                nameof(productId));

        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.",
                nameof(quantity));

        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.",
                nameof(quantity));

        Quantity += quantity;
    }

    public void DecreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.",
                nameof(quantity));

        if (Quantity - quantity <= 0)
            throw new InvalidOperationException(
                "Order item quantity cannot become zero.");

        Quantity -= quantity;
    }
}