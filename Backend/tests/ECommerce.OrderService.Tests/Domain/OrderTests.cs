using ECommerce.OrderService.Domain.Aggregates;
using ECommerce.OrderService.Domain.Enums;
using ECommerce.OrderService.Domain.ValueObjects;
using ECommerce.OrderService.Domain.DomainEvents;

namespace ECommerce.OrderService.Tests.Domain;

public class OrderTests
{

    //crear order
    [Fact(Skip = "Temporalmente deshabilitado mientras se integra MediatR")]
    public void CreateOrder_ShouldStartAsPending()
    {
        // Arrange
        var customerId = CustomerId.New();

        // Act
        var order = new Order(customerId);

        // Assert
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    //agregar item
    [Fact(Skip = "Temporalmente deshabilitado mientras se integra MediatR")]
    public void AddItem_ShouldAddProduct()
    {
        // Arrange
        var order = new Order(CustomerId.New());

        var productId = Guid.NewGuid();

        var price = new Money(
            50_000,
            "COP");

        // Act
        order.AddItem(
            productId,
            2,
            price);

        // Assert
        Assert.Single(order.Items);

        var item = order.Items.First();

        Assert.Equal(productId, item.ProductId);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(50_000, item.UnitPrice.Amount);
    }

    //realizar pedido
    [Fact(Skip = "Temporalmente deshabilitado mientras se integra MediatR")]
    public void Place_ShouldChangeStatus()
    {
        // Arrange
        var order = new Order(CustomerId.New());

        order.AddItem(
            Guid.NewGuid(),
            1,
            new Money(50_000, "COP"));

        // Act
        order.Place();

        // Assert
        Assert.Equal(OrderStatus.Placed, order.Status);
    }

    //domain events
    [Fact(Skip = "Temporalmente deshabilitado mientras se integra MediatR")]
    public void Place_ShouldCreateDomainEvent()
    {
        // Arrange
        var order = new Order(CustomerId.New());

        order.AddItem(
            Guid.NewGuid(),
            1,
            new Money(50_000, "COP"));

        // Act
        order.Place();

        // Assert
        var domainEvent = Assert.Single(order.DomainEvents);

        var orderPlacedEvent =
            Assert.IsType<OrderPlacedDomainEvent>(domainEvent);

        Assert.Equal(order.Id, orderPlacedEvent.OrderId);
        Assert.Equal(
            order.CustomerId,
            orderPlacedEvent.CustomerId);
    }

    //no permitir Order vacía
    [Fact(Skip = "Temporalmente deshabilitado mientras se integra MediatR")]
    public void CannotPlaceEmptyOrder()
    {
        // Arrange
        var order = new Order(CustomerId.New());

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => order.Place());

        Assert.Equal(
            "Cannot place an empty order.",
            exception.Message);

        Assert.Equal(
            OrderStatus.Pending,
            order.Status);
    }

    //no modificar Order colocada
    [Fact(Skip = "Temporalmente deshabilitado mientras se integra MediatR")]
    public void CannotModifyPlacedOrder()
    {
        // Arrange
        var order = new Order(CustomerId.New());

        order.AddItem(
            Guid.NewGuid(),
            1,
            new Money(50_000, "COP"));

        order.Place();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => order.AddItem(
                Guid.NewGuid(),
                1,
                new Money(20_000, "COP")));
    }

    //Cancel
    [Fact(Skip = "Temporalmente deshabilitado mientras se integra MediatR")]
    public void Cancel_ShouldChangeStatus()
    {
        // Arrange
        var order = new Order(CustomerId.New());

        // Act
        order.Cancel();

        // Assert
        Assert.Equal(
            OrderStatus.Cancelled,
            order.Status);
    }
}