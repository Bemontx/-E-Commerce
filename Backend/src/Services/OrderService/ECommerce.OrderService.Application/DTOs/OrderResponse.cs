namespace ECommerce.OrderService.Application.DTOs;

public sealed record OrderResponse(
    Guid Id,
    Guid CustomerId,
    string Status,
    decimal Total,
    string Currency,
    IReadOnlyCollection<OrderItemResponse> Items);