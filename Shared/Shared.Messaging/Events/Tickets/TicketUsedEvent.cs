namespace Shared.Messaging.Events.Tickets
{
    public class TicketUsedEvent : BaseTicketEvent
    {
        public Guid TicketId { get; set; }
    }
}
