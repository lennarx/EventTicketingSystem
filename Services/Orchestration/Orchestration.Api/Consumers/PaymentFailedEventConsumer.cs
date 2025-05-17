using MassTransit;
using Orchestration.Api.Services;
using Shared.Messaging.Events.Payments;

namespace Orchestration.Api.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly PurchaseSagaOrchestrator _orchestrator;

        public PaymentFailedEventConsumer(PurchaseSagaOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            await _orchestrator.HandlePaymentFailedAsync(context.Message);
        }
    }
}
