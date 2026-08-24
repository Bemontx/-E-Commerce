using ECommerce.OrderService.Domain.Aggregates;
using MediatR;

namespace ECommerce.OrderService.Application.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    Guid CustomerId) : IRequest<Order>;