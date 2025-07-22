namespace Shared.Contracts.Events;

public record OrderPlacedEvent
(
    Guid OrderId,
    Guid CustomerId,
    decimal Amount,
    List<OrderItemDto> Items
);

public record OrderItemDto
(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice
);
