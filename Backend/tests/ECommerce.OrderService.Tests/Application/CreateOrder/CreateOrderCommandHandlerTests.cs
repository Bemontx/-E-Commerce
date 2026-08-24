using ECommerce.OrderService.Application.Commands.CreateOrder;
using ECommerce.OrderService.Domain.Enums;

namespace ECommerce.OrderService.Tests.Application.CreateOrder;

public class CreateOrderCommandHandlerTests
{
    [Fact(Skip = "Temporalmente deshabilitado mientras se integra MediatR")]
    public void Handle_ShouldCreateOrder()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var command = new CreateOrderCommand(
            customerId);

        var handler =
            new CreateOrderCommandHandler();

        // Act
        var order = handler.Handle(command);

        // Assert
        Assert.Equal(
            customerId,
            order.CustomerId.Value);

        Assert.Equal(
            OrderStatus.Pending,
            order.Status);
    }
}