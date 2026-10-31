using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; } // thanh cong that bai
    }
}