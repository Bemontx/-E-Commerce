using MediatR;

namespace ECommerce.OrderService.Application.Commands.AddOrderItem;

public sealed record AddOrderItemCommand(
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    string Currency) : IRequest;