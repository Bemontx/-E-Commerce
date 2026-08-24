namespace ECommerce.OrderService.Application.DTOs;

public sealed record OrderItemResponse(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    string Currency);