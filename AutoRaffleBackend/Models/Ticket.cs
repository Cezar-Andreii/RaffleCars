namespace AutoRaffleBackend.Models
{
    public class Ticket
    {
        public int Id { get; set; } // Identificator unic al biletului
        public int CarId { get; set; } // ID-ul mașinii asociate
        public string BuyerName { get; set; } // Numele cumpărătorulu
    }
}
