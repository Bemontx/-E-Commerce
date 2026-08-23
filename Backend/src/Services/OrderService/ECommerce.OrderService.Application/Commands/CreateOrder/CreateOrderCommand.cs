namespace ECommerce.OrderService.Application.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    Guid CustomerId);