using AutoRaffleBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoRaffleBackend
{
     public class RaffleDbContext : DbContext
    {
        public RaffleDbContext(DbContextOptions<RaffleDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
