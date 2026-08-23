using ECommerce.OrderService.Domain.Aggregates;
using ECommerce.OrderService.Domain.ValueObjects;

namespace ECommerce.OrderService.Application.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler
{
    public Order Handle(CreateOrderCommand command)
    {
        var customerId =
            new CustomerId(command.CustomerId);

        var order = new Order(customerId);

        return order;
    }
}