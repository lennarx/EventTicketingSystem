using MassTransit;
using Shared.Messaging.Commands.Tickets;
using Shared.Messaging.Events.Tickets;
using Tickets.Application.Services;

namespace Tickets.Api.Consumers
{
    public class ConfirmReservationCommandConsumer : IConsumer<ConfirmReservationCommand>
    {
        private readonly ITicketService _ticketService;
        private readonly IPublishEndpoint _publishEndpoint;
        public ConfirmReservationCommandConsumer(ITicketService ticketService, IPublishEndpoint publishEndpoint)
        {
            _ticketService = ticketService;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<ConfirmReservationCommand> context)
        {
            var command = context.Message;
            var ticketConfirmation = await _ticketService.ConfirmReservationAsync(command.ReservationId);

            await _publishEndpoint.Publish(new ReservationConfirmedEvent
            {
                ReservationId = command.ReservationId,
                UserId = ticketConfirmation.UserId,
            });
        }
    }
}
