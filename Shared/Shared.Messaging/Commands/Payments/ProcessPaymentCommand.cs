namespace Shared.Messaging.Commands.Payments
{
    public class ProcessPaymentCommand
    {
        public Guid ReservationId { get; set; }
        public Guid UserId { get; set; }
        public int Amount { get; set; }
    }
}
