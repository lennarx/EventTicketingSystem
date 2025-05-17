namespace Shared.Messaging.Events.Payments
{
    public class PaymentFailedEvent
    {
        public Guid ReservationId { get; set; }
        public Guid UserId { get; set; }
        public int Amount { get; set; }
    }
}
