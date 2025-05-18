namespace Tickets.Application.Dtos
{
    public class ReservationDto
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public Guid ReservationId { get; set; }
        public string ReservationStatus { get; set; }
    }
}
