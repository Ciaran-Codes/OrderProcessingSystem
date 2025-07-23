using MassTransit;
using Shared.Contracts.Events;

namespace NotificationService.Consumers;

public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
{
    public Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        var message = context.Message;

        Console.WriteLine($"[NotificationService] ❌ Payment FAILED for Order {message.OrderId}. Sending alert to customer.");
        // Could log or send a different notification

        return Task.CompletedTask;
    }
}
