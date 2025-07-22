using MassTransit;
using Shared.Contracts.Events;

namespace InventoryService.Consumers;

public class OrderPlacedEventConsumer : IConsumer<OrderPlacedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderPlacedEventConsumer(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
    {
        var message = context.Message;

        Console.WriteLine($"[InventoryService] Adjusting inventory for Order {message.OrderId}");

        var adjustments = message.Items.Select(item =>
            new InventoryAdjustmentItem(
                item.ProductId,
                QuantityAdjusted: item.Quantity,
                NewStockLevel: SimulateNewStockLevel(item.ProductId, item.Quantity)
            )
        ).ToList();

        var inventoryAdjustedEvent = new InventoryAdjustedEvent(
            message.OrderId,
            adjustments,
            DateTime.UtcNow
        );

        await _publishEndpoint.Publish(inventoryAdjustedEvent);
    }

    private int SimulateNewStockLevel(Guid productId, int quantityOrdered)
    {
        // Dummy simulation - normally you'd read from DB
        var initialStock = 100; // assume 100 in stock
        var newLevel = initialStock - quantityOrdered;

        return newLevel > 0 ? newLevel : 0;
    }
}
