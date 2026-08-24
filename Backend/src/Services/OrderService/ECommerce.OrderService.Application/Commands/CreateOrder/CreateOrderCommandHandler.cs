using ECommerce.OrderService.Application.DTOs;
using ECommerce.OrderService.Application.Interfaces;
using ECommerce.OrderService.Domain.Aggregates;
using ECommerce.OrderService.Domain.ValueObjects;
using MediatR;

namespace ECommerce.OrderService.Application.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = new Order(
            new CustomerId(request.CustomerId));

        await _orderRepository.AddAsync(
            order,
            cancellationToken);

        return MapToResponse(order);
    }

    private static OrderResponse MapToResponse(Order order)
    {
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
            .ToList());
    }
}