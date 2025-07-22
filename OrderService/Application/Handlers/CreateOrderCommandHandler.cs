using MassTransit;
using MediatR;
using OrderService.Application.Commands;
using Shared.Contracts.Events;

namespace OrderService.Application.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateOrderCommandHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
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

        await _publishEndpoint.Publish(orderPlacedEvent, cancellationToken);

        return orderId;
    }
}
