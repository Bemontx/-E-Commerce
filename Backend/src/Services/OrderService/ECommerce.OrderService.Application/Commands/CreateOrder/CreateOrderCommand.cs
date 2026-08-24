using ECommerce.OrderService.Application.DTOs;
using MediatR;

namespace ECommerce.OrderService.Application.Commands.CreateOrder;

public sealed record CreateOrderCommand(
    Guid CustomerId) : IRequest<OrderResponse>;