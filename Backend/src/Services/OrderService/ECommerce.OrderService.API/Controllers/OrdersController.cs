using ECommerce.OrderService.Application.Commands.AddOrderItem;
using ECommerce.OrderService.Application.Commands.CreateOrder;
using ECommerce.OrderService.Application.Queries.GetOrder;
using ECommerce.OrderService.Application.Queries.GetOrders;
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
    public async Task<IActionResult> GetOrder(
       Guid id,
       CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetOrderQuery(id),
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(
    CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetOrdersQuery(),
            cancellationToken);

        return Ok(result);
    }
}

public sealed record AddOrderItemRequest(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    string Currency);