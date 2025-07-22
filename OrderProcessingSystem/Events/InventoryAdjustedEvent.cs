namespace Shared.Contracts.Events;

public record InventoryAdjustedEvent
(
    Guid OrderId,
    List<InventoryAdjustmentItem> Items,
    DateTime AdjustedAt
);

public record InventoryAdjustmentItem
(
    Guid ProductId,
    int QuantityAdjusted,
    int NewStockLevel
);
