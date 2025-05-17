namespace Shared.Messaging.Events.Tickets
{
    public class TicketReservedEvent : BaseTicketEvent
    {
        public Guid EventId { get; set; }
    }
}
