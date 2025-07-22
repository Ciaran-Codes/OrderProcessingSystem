using MassTransit;
using MediatR;
using OrderService.Application.Commands;
using OrderService.Domain.Entities;
using OrderService.Persistence;
using Shared.Contracts.Events;

namespace OrderService.Application.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly OrderDbContext _dbContext;

    public CreateOrderCommandHandler(IPublishEndpoint publishEndpoint, OrderDbContext dbContext)
    {
        _publishEndpoint = publishEndpoint;
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();

        var eventItems = request.Items.Select(item =>
            new OrderItemDto(item.ProductId, item.Quantity, item.UnitPrice)).ToList();

        var orderPlacedEvent = new OrderPlacedEvent(
            orderId,
            request.CustomerId,
            request.Amount,
            eventItems
        );

        var order = new Order
        {
            Id = orderId,
            CustomerId = request.CustomerId,
            Amount = request.Amount,
            CreatedAt = DateTime.UtcNow,
            Items = request.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _publishEndpoint.Publish(orderPlacedEvent, cancellationToken);

        return orderId;
    }
}
