using MassTransit;
using Shared.Messaging.Commands.Payments;
using Shared.Messaging.Commands.Tickets;
using Shared.Messaging.Events.Payments;
using Shared.Messaging.Events.Tickets;

namespace Orchestration.Api.Services
{
    public class PurchaseSagaOrchestrator
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PurchaseSagaOrchestrator(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task HandleTicketReservedEvent(TicketReservedEvent @event)
        {
            var processPayment = new ProcessPaymentCommand
            {
                ReservationId = @event.ReservationId,
                UserId = @event.UserId,
                Amount = 1000 // dummy
            };

            await _publishEndpoint.Publish(processPayment);
        }

        public async Task HandlePaymentSucceededAsync(PaymentSucceededEvent @event)
        {
            var confirmReservation = new ConfirmReservationCommand
            {
                ReservationId = @event.ReservationId,
            };

            await _publishEndpoint.Publish(confirmReservation);
        }

        public async Task HandlePaymentFailedAsync(PaymentFailedEvent message)
        {
            var cancelReservation = new CancelReservationCommand
            {
                ReservationId = message.ReservationId,
            };

            await _publishEndpoint.Publish(cancelReservation);
        }

        public async Task HandleReservationConfirmedEvent(ReservationConfirmedEvent message)
        {
            Console.WriteLine($"[SAGA COMPLETE] Reservation confirmed for UserId: {message.UserId} - ReservationId: {message.ReservationId}");
        }

        public async Task HandleReservationCanceledEvent(ReservationCanceledEvent message)
        {
            Console.WriteLine($"[SAGA COMPLETE] Reservation canceled for UserId: {message.UserId} - ReservationId: {message.ReservationId}");
        }
    }
}
