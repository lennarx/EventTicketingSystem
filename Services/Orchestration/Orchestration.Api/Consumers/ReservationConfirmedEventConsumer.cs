using MassTransit;
using Orchestration.Api.Services;
using Shared.Messaging.Events.Tickets;

namespace Orchestration.Api.Consumers
{
    public class ReservationConfirmedEventConsumer : IConsumer<ReservationConfirmedEvent>
    {
        private readonly PurchaseSagaOrchestrator _orchestrator;
        public ReservationConfirmedEventConsumer(PurchaseSagaOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public async Task Consume(ConsumeContext<ReservationConfirmedEvent> context)
        {
            await _orchestrator.HandleReservationConfirmedEvent(context.Message);
        }
    }
}
