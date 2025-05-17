namespace Tickets.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
