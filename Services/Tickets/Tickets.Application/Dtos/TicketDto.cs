namespace Tickets.Application.Dtos
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid? EventId { get; set; }
        public string TicketStatus { get; set; }
    }
}
