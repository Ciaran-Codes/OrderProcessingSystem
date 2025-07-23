using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Commands;
using OrderService.Persistence;

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
    public async Task<IActionResult> GetOrder(Guid id, [FromServices] OrderDbContext dbContext)
    {
        var order = await dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound();

        return Ok(new
        {
            order.Id,
            order.CustomerId,
            order.Amount,
            order.CreatedAt,
            Items = order.Items.Select(i => new
            {
                i.ProductId,
                i.Quantity,
                i.UnitPrice
            })
        });
    }
}
