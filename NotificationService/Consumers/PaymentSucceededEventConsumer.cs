using MassTransit;
using Shared.Contracts.Events;

namespace NotificationService.Consumers;

public class PaymentSucceededEventConsumer : IConsumer<PaymentSucceededEvent>
{
    public Task Consume(ConsumeContext<PaymentSucceededEvent> context)
    {
        var message = context.Message;

        Console.WriteLine($"[NotificationService] ✅ Payment successful for Order {message.OrderId}. Sending confirmation email...");
        // Pretend to send an email or SMS here

        return Task.CompletedTask;
    }
}
