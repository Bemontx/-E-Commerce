using ECommerce.OrderService.Application.DTOs;
using ECommerce.OrderService.Application.Interfaces;
using ECommerce.OrderService.Domain.ValueObjects;
using MediatR;

namespace ECommerce.OrderService.Application.Queries.GetOrder;

public sealed class GetOrderQueryHandler
    : IRequestHandler<GetOrderQuery, OrderResponse?>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse?> Handle(
        GetOrderQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Envolver el Guid en el Value Object OrderId
        var orderId = new OrderId(request.OrderId);

        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);

        if (order is null)
            return null;

        // 2. Mapear usando el constructor posicional del record
        return new OrderResponse(
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
                    item.UnitPrice.Currency
                ))
                .ToList()
        );
    }
}