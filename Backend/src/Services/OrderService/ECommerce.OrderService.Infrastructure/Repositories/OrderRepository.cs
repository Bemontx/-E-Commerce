using ECommerce.OrderService.Application.Interfaces;
using ECommerce.OrderService.Domain.Aggregates;
using ECommerce.OrderService.Domain.ValueObjects;
using ECommerce.OrderService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.OrderService.Infrastructure.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _dbContext;

    public OrderRepository(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> GetByIdAsync(
        OrderId orderId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(
                x => x.Id == orderId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(
    CancellationToken cancellationToken)
    {
        return await _dbContext.Orders
            .Include(x => x.Items)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Orders.AddAsync(
            order,
            cancellationToken);

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }

    public async Task SaveAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }
}