using ECommerce.OrderService.Application.DTOs;
using MediatR;

namespace ECommerce.OrderService.Application.Queries.GetOrder;

public sealed record GetOrderQuery(Guid OrderId) : IRequest<OrderResponse>;