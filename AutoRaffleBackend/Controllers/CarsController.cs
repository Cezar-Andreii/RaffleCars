using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AutoRaffleBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AutoRaffleBackend
{
    
    namespace AutoRaffle.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class CarsController : ControllerBase
        {
            // Lista mock-up pentru stocarea mașinilor
            private static List<Car> cars = new List<Car>
        {
            new Car { Id = 1, Make = "Toyota", Model = "Corolla", Year = 2020, Price = 15000, ImageUrl = "https://example.com/toyota.jpg" },
            new Car { Id = 2, Make = "Honda", Model = "Civic", Year = 2021, Price = 18000, ImageUrl = "https://example.com/honda.jpg" }
        };

            private static List<Ticket> tickets = new List<Ticket>();
            private readonly RaffleDbContext _context;

            public CarsController(RaffleDbContext context)
            {
                _context = context;
            }

            // Endpoint pentru afișarea tuturor mașinilor
            [HttpGet]
            public IActionResult GetCars()
            {
                return Ok(cars);
            }

            // Endpoint pentru afișarea unei mașini după ID
            [HttpGet("{id}")]
            public IActionResult GetCarById(int id)
            {
                var car = cars.FirstOrDefault(c => c.Id == id);
                if (car == null)
                {
                    return NotFound("Car not found.");
                }
                return Ok(car);
            }

            // Endpoint pentru adăugarea unei mașini noi
            [HttpPost]
            public IActionResult AddCar([FromBody] Car car)
            {
                car.Id = cars.Count + 1; // Generare ID unic
                cars.Add(car);
                return CreatedAtAction(nameof(GetCarById), new { id = car.Id }, car);
            }

            // Endpoint pentru ștergerea unei mașini
            [HttpDelete("{id}")]
            public IActionResult DeleteCar(int id)
            {
                var car = cars.FirstOrDefault(c => c.Id == id);
                if (car == null)
                {
                    return NotFound("Car not found.");
                }
                cars.Remove(car);
                return NoContent();
            }

            // Endpoint pentru filtrarea mașinilor (după marcă și/sau preț maxim)
            [HttpGet("filter")]
            public IActionResult FilterCars([FromQuery] string? make, [FromQuery] decimal? maxPrice)
            {
                var filteredCars = cars.Where(car =>
                    (string.IsNullOrEmpty(make) || car.Make.ToLower() == make.ToLower()) &&
                    (!maxPrice.HasValue || car.Price <= maxPrice)).ToList();

                return Ok(filteredCars);
            }


            [HttpPost("{id}/tickets")]
            public async Task<IActionResult> GenerateTickets(int id, [FromQuery] decimal ticketPrice)
            {
                var car = await _context.Cars.FindAsync(id);
                if (car == null) return NotFound("Car not found.");

                int ticketCount = (int)(car.Price / ticketPrice);
                var tickets = new List<Ticket>();

                for (int i = 0; i < ticketCount; i++)
                {
                    tickets.Add(new Ticket { CarId = car.Id, BuyerName = null });
                }

                _context.Tickets.AddRange(tickets);
                await _context.SaveChangesAsync();

                return Ok(tickets);
            }



            [HttpPost("{id}/buy-ticket")]
            public async Task<IActionResult> BuyTicket(int id, [FromBody] string buyerName)
            {
                var ticket = await _context.Tickets
                    .Where(t => t.CarId == id && t.BuyerName == null)
                    .FirstOrDefaultAsync();

                if (ticket == null) return NotFound("No tickets available for this car.");

                ticket.BuyerName = buyerName;
                await _context.SaveChangesAsync();

                return Ok(ticket);
            }


            [HttpGet("{id}/tickets/available")]
            public IActionResult GetAvailableTickets(int id)
            {
                var availableTickets = tickets.Where(t => t.CarId == id && t.BuyerName == null).ToList();

                if (!availableTickets.Any())
                {
                    return NotFound("No tickets available for this car.");
                }

                return Ok(availableTickets);
            }


            public class TicketPurchaseRequest
            {
                public string buyerName { get; set; }
            }
            
        }

    }

}
