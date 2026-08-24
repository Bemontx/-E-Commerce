using ECommerce.OrderService.Domain.Aggregates;
using ECommerce.OrderService.Domain.ValueObjects;

namespace ECommerce.OrderService.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(
        OrderId orderId,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    Task SaveAsync(Order order, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<Order>> GetAllAsync(
    CancellationToken cancellationToken);
}