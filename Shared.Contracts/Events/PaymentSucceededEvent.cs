namespace Shared.Contracts.Events;

public record PaymentSucceededEvent
(
    Guid OrderId,
    decimal Amount,
    DateTime ProcessedAt
);