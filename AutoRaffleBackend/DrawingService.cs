using Microsoft.EntityFrameworkCore;

namespace AutoRaffleBackend
{
    public class DrawingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DrawingService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RaffleDbContext>();
                var now = DateTime.UtcNow;

                // Găsește mașinile al căror timp de extragere a fost atins
                var carsToDraw = await context.Cars
                    .Where(c => c.DrawingTime <= now && c.DrawingTime != null)
                    .ToListAsync();

                foreach (var car in carsToDraw)
                {
                    var tickets = context.Tickets
                        .Where(t => t.CarId == car.Id && t.BuyerName != null)
                        .ToList();

                    if (tickets.Any())
                    {
                        var random = new Random();
                        var winner = tickets[random.Next(tickets.Count)];
                        // Poți salva câștigătorul sau să faci ceva cu datele lui
                        Console.WriteLine($"Câștigătorul pentru mașina {car.Make} {car.Model} este {winner.BuyerName}!");

                        // Poți marca mașina ca având extragerea finalizată
                        car.DrawingTime = null; // Marchezi extragerea ca finalizată
                    }
                }

                await context.SaveChangesAsync();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Rulează verificarea la fiecare minut
            }
        }
    }

}
