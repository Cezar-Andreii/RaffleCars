namespace AutoRaffleBackend.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? DrawingTime { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
