using ECommerce.OrderService.Application.DTOs;
using MediatR;

namespace ECommerce.OrderService.Application.Queries.GetOrders;

public sealed record GetOrdersQuery()
    : IRequest<IReadOnlyList<OrderResponse>>;