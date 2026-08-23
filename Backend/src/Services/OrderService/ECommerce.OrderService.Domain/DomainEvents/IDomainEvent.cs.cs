namespace ECommerce.OrderService.Domain.DomainEvents;

public interface IDomainEvent
{
    DateTime OcurredOnUtc { get; }
}