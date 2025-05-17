namespace Shared.Messaging.Events.Tickets
{
    public abstract class BaseTicketEvent
    {        
        public Guid UserId { get; set; }
        public Guid ReservationId { get; set; }

        public int Quantity { get; set; }
    }
}
