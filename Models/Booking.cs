using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public DateTime BookingTime { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}