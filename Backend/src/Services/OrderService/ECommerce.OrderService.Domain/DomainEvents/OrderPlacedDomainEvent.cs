using ECommerce.OrderService.Domain.ValueObjects;

namespace ECommerce.OrderService.Domain.DomainEvents;

public sealed record OrderPlacedDomainEvent(OrderId OrderId, CustomerId CustomerId, DateTime OcurredOnUtc) : IDomainEvent;