using MassTransit;
using Orchestration.Api.Services;
using Shared.Messaging.Events.Payments;

namespace Orchestration.Api.Consumers
{
    public class PaymentSucceededEventConsumer : IConsumer<PaymentSucceededEvent>
    {
        private readonly PurchaseSagaOrchestrator _orchestrator;

        public PaymentSucceededEventConsumer(PurchaseSagaOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public async Task Consume(ConsumeContext<PaymentSucceededEvent> context)
        {
            await _orchestrator.HandlePaymentSucceededAsync(context.Message);
        }
    }
}
