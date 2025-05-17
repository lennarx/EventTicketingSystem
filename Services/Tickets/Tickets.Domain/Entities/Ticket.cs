using Tickets.Domain.Enums;

namespace Tickets.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public virtual User User { get; set; }
        public Guid UserId { get; set; }
        public TicketStatusEnum Status { get; set; }
        public Guid ReservationId { get; set; }
        public Guid EventId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
