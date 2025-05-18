using MassTransit;
using Shared.Messaging.Commands.Tickets;
using Shared.Messaging.Events.Tickets;
using Tickets.Application.Services;

namespace Tickets.Api.Consumers
{
    public class CancelReservationCommandConsumer : IConsumer<CancelReservationCommand>
    {
        private readonly IReservationService _ticketService;
        private readonly IPublishEndpoint _publishEndpoint;
        public CancelReservationCommandConsumer(IReservationService ticketService, IPublishEndpoint publishEndpoint)
        {
            _ticketService = ticketService;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<CancelReservationCommand> context)
        {
            var command = context.Message;
            var reservationConfirmation = await _ticketService.CancelReservationAsync(command.ReservationId);

            if (reservationConfirmation.ReservationId != Guid.Empty)
                await _publishEndpoint.Publish(new ReservationCanceledEvent
                {
                    ReservationId = command.ReservationId,
                    UserId = reservationConfirmation.UserId,
                });
        }
    }
}
