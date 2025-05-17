using MassTransit;
using Orchestration.Api.Services;
using Shared.Messaging.Events.Tickets;

namespace Orchestration.Api.Consumers
{
    public class ReservationCanceledEventConsumer : IConsumer<ReservationCanceledEvent>
    {
        private readonly PurchaseSagaOrchestrator _orchestrator;
        public ReservationCanceledEventConsumer(PurchaseSagaOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }
        public async Task Consume(ConsumeContext<ReservationCanceledEvent> context)
        {
            await _orchestrator.HandleReservationCanceledEvent(context.Message);
        }
    }
}
