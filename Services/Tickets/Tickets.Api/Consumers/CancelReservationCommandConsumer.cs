using MassTransit;
using Shared.Messaging.Commands.Tickets;
using Shared.Messaging.Events.Tickets;
using Tickets.Application.Services;

namespace Tickets.Api.Consumers
{
    public class CancelReservationCommandConsumer : IConsumer<CancelReservationCommand>
    {
        private readonly ITicketService _ticketService;
        private readonly IPublishEndpoint _publishEndpoint;
        public CancelReservationCommandConsumer(ITicketService ticketService, IPublishEndpoint publishEndpoint)
        {
            _ticketService = ticketService;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<CancelReservationCommand> context)
        {
            var command = context.Message;
            var ticketConfirmation = await _ticketService.CancelReservationAsync(command.ReservationId);

            await _publishEndpoint.Publish(new ReservationCanceledEvent
            {
                ReservationId = command.ReservationId,
                UserId = ticketConfirmation.UserId,
            });
        }
    }
}
