using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AutoRaffleBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AutoRaffleBackend
{
    
    namespace AutoRaffle.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class CarsController : ControllerBase
        {
            private static List<Car> cars = new List<Car>
        {
        };

            private static List<Ticket> tickets = new List<Ticket>();
            private readonly RaffleDbContext _context;

            private readonly IConfiguration _configuration;

            public CarsController(IConfiguration configuration, RaffleDbContext context)
            {
                _configuration = configuration;
                _context = context;
            }


            [HttpGet]
            public async Task<IActionResult> GetAllCars()
            {
                var cars = await _context.Cars.ToListAsync(); 
                return Ok(cars);
            }

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

            [HttpPost]
            public async Task<IActionResult> AddCar([FromBody] Car car)
            {
                car.DrawingTime = DateTime.UtcNow.AddMinutes(5);
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCarById), new { id = car.Id }, car);
            }

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
                    tickets.Add(new Ticket {CarId = car.Id, BuyerName = null });
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
            public async Task<IActionResult> GetAvailableTickets(int id)
            {
                var availableTickets = await _context.Tickets
                    .Where(t => t.CarId == id && t.BuyerName == null) 
                    .ToListAsync();

                if (!availableTickets.Any())
                {
                    return NotFound("No tickets available for this car.");
                }

                return Ok(availableTickets);
            }

            [HttpGet("{id}/winner")]
            public IActionResult GetWinner(int id)
            {
                var ticketsForCar = _context.Tickets
                    .Where(t => t.CarId == id && t.BuyerName != null)
                    .ToList();

                if (!ticketsForCar.Any())
                {
                    return NotFound("No tickets sold for this car.");
                }

                var random = new Random();
                var winner = ticketsForCar[random.Next(ticketsForCar.Count)];

                // Creează mesajul câștigătorului
                var message = $"Felicitări, {winner.BuyerName}! Ai câștigat o mașină {winner.Car.Make} {winner.Car.Model}!";
                Console.WriteLine($"Mesaj pentru câștigător: {message}");

                // Comentează trimiterea SMS-ului pentru testare
                // smsService.SendSms("+40[numar_verificat]", message);

                return Ok(new { Message = "Câștigător selectat, dar SMS-ul nu a fost trimis.", Winner = winner });
            }



            [HttpGet("test-sms")]
            public IActionResult TestSms()
            {
                // Preia valorile din configurație
                var accountSid = _configuration["Twilio:AccountSid"];
                var authToken = _configuration["Twilio:AuthToken"];
                var fromPhoneNumber = _configuration["Twilio:PhoneNumber"];

                // Afișează valorile în consolă pentru debugging
                Console.WriteLine($"Account SID: {accountSid}");
                Console.WriteLine($"Auth Token: {authToken}");
                Console.WriteLine($"From Phone Number: {fromPhoneNumber}");

                // Verifică dacă valorile sunt valide
                if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(fromPhoneNumber))
                {
                    return BadRequest("Una sau mai multe valori Twilio sunt null sau goale.");
                }

                // Încearcă să trimiți un SMS
                try
                {
                    var smsService = new SmsService(_configuration);
                    smsService.SendSms("+40761234567", "Mesaj de test din aplicație");
                    return Ok("SMS trimis cu succes!");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Eroare: {ex.Message}");
                }
            }




            public class TicketPurchaseRequest
            {
                public string buyerName { get; set; }
            }
            
        }

    }

}
