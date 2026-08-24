using ECommerce.OrderService.Application.Interfaces;
using ECommerce.OrderService.Domain.Aggregates;
using ECommerce.OrderService.Domain.ValueObjects;

namespace ECommerce.OrderService.Tests.Application.Fakes;

public sealed class FakeOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = [];

    public Task AddAsync(
        Order order,
        CancellationToken cancellationToken)
    {
        _orders.Add(order);

        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(
        OrderId orderId,
        CancellationToken cancellationToken)
    {
        var order = _orders.FirstOrDefault(
            x => x.Id == orderId);

        return Task.FromResult(order);
    }

    public Task SaveAsync(
        Order order,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}