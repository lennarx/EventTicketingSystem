using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;

namespace Tickets.Infrastructure
{
    public class TicketsDbContext : DbContext
    {
        public TicketsDbContext(DbContextOptions<TicketsDbContext> options)
            : base(options)
        {
        }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserId);
                entity.HasIndex(x => x.ReservationId);
                entity.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                    .ValueGeneratedOnAdd();
                entity.Property(x => x.UpdatedAt);
            });

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
        }
    }
}
