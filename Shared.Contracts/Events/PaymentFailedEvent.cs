namespace Shared.Contracts.Events;

public record PaymentFailedEvent
(
    Guid OrderId,
    string Reason,
    DateTime FailedAt
);