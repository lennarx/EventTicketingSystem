using MassTransit;
using Shared.Messaging.Commands.Tickets;
using Shared.Messaging.Events.Tickets;
using Tickets.Application.Services;

namespace Tickets.Api.Consumers
{
    public class CreateReservationCommandConsumer : IConsumer<CreateReservationCommand>
    {
        private readonly ITicketService _ticketService;
        private readonly IPublishEndpoint _publishEndpoint;
        public CreateReservationCommandConsumer(ITicketService ticketService, IPublishEndpoint publishEndpoint)
        {
            _ticketService = ticketService;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<CreateReservationCommand> context)
        {
            var command = context.Message;
            var ticketsConfirmation = await _ticketService.ReserveTicketsAsync(command.EventId, command.UserId, command.Quantity);

            await _publishEndpoint.Publish(new TicketReservedEvent
            {
                ReservationId = ticketsConfirmation.First().ReservationId ?? Guid.Empty,
                EventId = command.EventId,
                UserId = command.UserId,
                Quantity = command.Quantity,
            });
        }
    }
}
