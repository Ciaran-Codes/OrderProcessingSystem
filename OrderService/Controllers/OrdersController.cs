using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var orderId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrder), new { id = orderId }, new { OrderId = orderId });
    }

    [HttpGet("{id}")]
    public IActionResult GetOrder(Guid id)
    {
        // Placeholder - we don't have persistence yet
        return Ok(new { OrderId = id, Status = "Mocked - Not Implemented" });
    }
}
