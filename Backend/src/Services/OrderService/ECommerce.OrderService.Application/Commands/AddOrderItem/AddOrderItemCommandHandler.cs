using ECommerce.OrderService.Application.Interfaces;
using ECommerce.OrderService.Domain.ValueObjects;
using MediatR;

namespace ECommerce.OrderService.Application.Commands.AddOrderItem;

public sealed class AddOrderItemCommandHandler
    : IRequestHandler<AddOrderItemCommand>
{
    private readonly IOrderRepository _orderRepository;

    public AddOrderItemCommandHandler(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(
        AddOrderItemCommand command,
        CancellationToken cancellationToken)
    {
        var orderId =
            new OrderId(command.OrderId);

        var order = await _orderRepository.GetByIdAsync(
            orderId,
            cancellationToken);

        if (order is null)
        {
            throw new InvalidOperationException(
                "Order was not found.");
        }

        var money = new Money(
            command.UnitPrice,
            command.Currency);

        order.AddItem(
            command.ProductId,
            command.Quantity,
            money);

        await _orderRepository.SaveAsync(
            order,
            cancellationToken);
    }
}