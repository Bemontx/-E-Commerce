using ECommerce.OrderService.Domain.Entities;
using ECommerce.OrderService.Domain.Enums;
using ECommerce.OrderService.Domain.ValueObjects;
using ECommerce.OrderService.Domain.DomainEvents;

namespace ECommerce.OrderService.Domain.Aggregates;

public sealed class Order
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly List<OrderItem> _items = new();

    public OrderId Id { get; private set; }

    public CustomerId CustomerId { get; private set; }

    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public IReadOnlyCollection<OrderItem> Items =>
        _items.AsReadOnly();

    public Money Total
    {
        get
        {
            if (_items.Count == 0)
                return new Money(0, "COP");

            var total = new Money(0, _items[0].UnitPrice.Currency);

            foreach (var item in _items)
            {
                total = total.Add(item.Total);
            }

            return total;
        }
    }

    private Order()
    {
    }

    public Order(CustomerId customerId)
    {
        if (customerId.Value == Guid.Empty)
            throw new ArgumentException(
                "CustomerId is required.",
                nameof(customerId));

        Id = OrderId.New();
        CustomerId = customerId;
        Status = OrderStatus.Pending;
    }

    public void AddItem(
        Guid productId,
        int quantity,
        Money unitPrice)
    {
        EnsurePending();

        var existingItem = _items
            .FirstOrDefault(x => x.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
            return;
        }

        _items.Add(
            new OrderItem(
                productId,
                quantity,
                unitPrice));
    }

    public void RemoveItem(Guid productId)
    {
        EnsurePending();

        var item = _items
            .FirstOrDefault(x => x.ProductId == productId);

        if (item is null)
            throw new InvalidOperationException(
                "Product does not exist in the order.");

        _items.Remove(item);
    }

    public void Place()
    {
        EnsurePending();

        if (_items.Count == 0)
            throw new InvalidOperationException(
                "Cannot place an empty order.");

        Status = OrderStatus.Placed;

        _domainEvents.Add(new OrderPlacedDomainEvent(Id,CustomerId,DateTime.UtcNow));
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException(
                "Order is already cancelled.");

        Status = OrderStatus.Cancelled;
    }

    private void EnsurePending()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException(
                "Order cannot be modified in its current state.");
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}