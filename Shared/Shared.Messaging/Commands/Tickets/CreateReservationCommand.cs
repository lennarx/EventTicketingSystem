namespace Shared.Messaging.Commands.Tickets
{
    public class CreateReservationCommand
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public int Quantity { get; set; }
    }
}
