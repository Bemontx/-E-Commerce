using ECommerce.OrderService.Application.Commands.AddOrderItem;
using ECommerce.OrderService.Application.Commands.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.OrderService.API.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var order = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetOrder),
            new { id = order.Id },
            order);
    }

    [HttpPost("{orderId:guid}/items")]
    public async Task<IActionResult> AddOrderItem(
        Guid orderId,
        [FromBody] AddOrderItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddOrderItemCommand(
            orderId,
            request.ProductId,
            request.Quantity,
            request.UnitPrice,
            request.Currency);

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetOrder(Guid id)
    {
        return Ok();
    }
}

public sealed record AddOrderItemRequest(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    string Currency);