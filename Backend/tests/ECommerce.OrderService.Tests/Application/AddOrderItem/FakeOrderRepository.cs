using ECommerce.OrderService.Application.Interfaces;
using ECommerce.OrderService.Domain.Aggregates;
using ECommerce.OrderService.Domain.ValueObjects;

namespace ECommerce.OrderService.Tests.Application.AddOrderItem;

public sealed class FakeOrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = new();

    public Task<Order?> GetByIdAsync(
        OrderId orderId,
        CancellationToken cancellationToken = default)
    {
        _orders.TryGetValue(
            orderId.Value,
            out var order);

        return Task.FromResult(order);
    }

    public Task AddAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        _orders[order.Id.Value] = order;

        return Task.CompletedTask;
    }

    public Task SaveAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        _orders[order.Id.Value] = order;

        return Task.CompletedTask;
    }
}