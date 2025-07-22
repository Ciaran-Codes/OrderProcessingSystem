namespace Shared.Contracts.Events;

public record InvoiceGeneratedEvent
(
    Guid OrderId,
    Guid InvoiceId,
    string InvoiceNumber,
    string InvoiceUrl,  // Could be a link to the PDF stored somewhere
    DateTime GeneratedAt
);