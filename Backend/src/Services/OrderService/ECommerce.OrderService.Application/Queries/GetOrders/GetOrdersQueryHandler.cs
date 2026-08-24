using ECommerce.OrderService.Application.DTOs;
using ECommerce.OrderService.Application.Interfaces;
using MediatR;

namespace ECommerce.OrderService.Application.Queries.GetOrders;

public sealed class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, IReadOnlyList<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IReadOnlyList<OrderResponse>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync(
            cancellationToken);

        return orders
            .Select(order => new OrderResponse(
                order.Id.Value,
                order.CustomerId.Value,
                order.Status.ToString(),
                order.Total.Amount,
                order.Total.Currency,
                order.Items
                    .Select(item => new OrderItemResponse(
                        item.ProductId,
                        item.Quantity,
                        item.UnitPrice.Amount,
                        item.Total.Amount,
                        item.UnitPrice.Currency))
                    .ToList()))
            .ToList();
    }
}