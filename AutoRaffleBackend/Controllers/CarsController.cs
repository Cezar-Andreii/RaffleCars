using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
        }

        // Modelul pentru mașini
        public class Car
        {
            public int Id { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
        }
    }

}
