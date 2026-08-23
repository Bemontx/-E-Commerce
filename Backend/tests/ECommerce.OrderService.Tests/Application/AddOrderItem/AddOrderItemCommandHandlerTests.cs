using ECommerce.OrderService.Application.Commands.AddOrderItem;
using ECommerce.OrderService.Domain.Aggregates;
using ECommerce.OrderService.Domain.ValueObjects;

namespace ECommerce.OrderService.Tests.Application.AddOrderItem;

public class AddOrderItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldAddItemToOrder()
    {
        // Arrange

        var repository = new FakeOrderRepository();

        var order = new Order(
            CustomerId.New());

        await repository.AddAsync(order);

        var command = new AddOrderItemCommand(
            order.Id.Value,
            Guid.NewGuid(),
            2,
            50_000,
            "COP");

        var handler =
            new AddOrderItemCommandHandler(repository);

        // Act

        await handler.Handle(command);

        // Assert

        var updatedOrder =
            await repository.GetByIdAsync(order.Id);

        Assert.NotNull(updatedOrder);

        Assert.Single(updatedOrder.Items);

        var item = updatedOrder.Items.First();

        Assert.Equal(2, item.Quantity);
        Assert.Equal(50_000, item.UnitPrice.Amount);
        Assert.Equal("COP", item.UnitPrice.Currency);
    }
}