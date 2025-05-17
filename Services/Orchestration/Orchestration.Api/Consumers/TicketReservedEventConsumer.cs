using MassTransit;
using Orchestration.Api.Services;
using Shared.Messaging.Events.Tickets;

namespace Orchestration.Api.Consumers
{
    public class TicketReservedEventConsumer : IConsumer<TicketReservedEvent>
    {
        private readonly PurchaseSagaOrchestrator _orchestrator;

        public TicketReservedEventConsumer(PurchaseSagaOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public async Task Consume(ConsumeContext<TicketReservedEvent> context)
        {
            var @event = context.Message;
            await _orchestrator.HandleTicketReservedEvent(@event);
        }
    }
}
