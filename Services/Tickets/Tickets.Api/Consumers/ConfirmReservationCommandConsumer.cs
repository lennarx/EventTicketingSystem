using MassTransit;
using Shared.Messaging.Commands.Tickets;
using Shared.Messaging.Events.Tickets;
using Tickets.Application.Services;

namespace Tickets.Api.Consumers
{
    public class ConfirmReservationCommandConsumer : IConsumer<ConfirmReservationCommand>
    {
        private readonly IReservationService _ticketService;
        private readonly IPublishEndpoint _publishEndpoint;
        public ConfirmReservationCommandConsumer(IReservationService ticketService, IPublishEndpoint publishEndpoint)
        {
            _ticketService = ticketService;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<ConfirmReservationCommand> context)
        {
            var command = context.Message;
            var reservationConfirmation = await _ticketService.ConfirmReservationAsync(command.ReservationId);

            if(reservationConfirmation.ReservationId != Guid.Empty)
            {
                await _publishEndpoint.Publish(new ReservationConfirmedEvent
                {
                    ReservationId = command.ReservationId,
                    UserId = reservationConfirmation.UserId,
                });
            }            
        }
    }
}
