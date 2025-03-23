using AutoRaffleBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoRaffleBackend
{
    public class RaffleDbContext : DbContext
    {
        public RaffleDbContext(DbContextOptions<RaffleDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().HasKey(c => c.Id);
            modelBuilder.Entity<Ticket>().HasKey(t => t.Id);
            modelBuilder.Entity<Ticket>()
                .Property(t => t.BuyerName)
                .IsRequired(false); // Permite valori NULL pentru BuyerName
        }
    }
}
