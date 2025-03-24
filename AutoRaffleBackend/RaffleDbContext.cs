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
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Car) // Relație către Car
                .WithMany(c => c.Tickets) // Mașina poate avea mai multe bilete
                .HasForeignKey(t => t.CarId);
            modelBuilder.Entity<Ticket>()
                .Property(t => t.BuyerName)
                .IsRequired(false); // Permite valori NULL pentru BuyerName
        }
    }
}
