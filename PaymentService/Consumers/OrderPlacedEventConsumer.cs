using MassTransit;
using Shared.Contracts.Events;

namespace PaymentService.Consumers;

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

        // Fake Payment Processing
        var paymentSuccessful = SimulatePaymentProcessing();

        if (paymentSuccessful)
        {
            var paymentSucceeded = new PaymentSucceededEvent(
                message.OrderId,
                message.Amount,
                DateTime.UtcNow
            );

            await _publishEndpoint.Publish(paymentSucceeded);
            Console.WriteLine($"[PaymentService] Payment succeeded for Order {message.OrderId}");
        }
        else
        {
            var paymentFailed = new PaymentFailedEvent(
                message.OrderId,
                "Insufficient funds (simulated)",
                DateTime.UtcNow
            );

            await _publishEndpoint.Publish(paymentFailed);
            Console.WriteLine($"[PaymentService] Payment FAILED for Order {message.OrderId}");
        }
    }

    private bool SimulatePaymentProcessing()
    {
        // Random success/fail
        return Random.Shared.Next(0, 2) == 1;
    }
}
