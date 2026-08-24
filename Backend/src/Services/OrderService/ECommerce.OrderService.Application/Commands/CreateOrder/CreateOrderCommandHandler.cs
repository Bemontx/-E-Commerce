using ECommerce.OrderService.Application.Interfaces;
using ECommerce.OrderService.Domain.Aggregates;
using ECommerce.OrderService.Domain.ValueObjects;
using MediatR;

namespace ECommerce.OrderService.Application.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var customerId =
            new CustomerId(command.CustomerId);

        var order = new Order(customerId);

        await _orderRepository.AddAsync(
            order,
            cancellationToken);

        return order;
    }
}