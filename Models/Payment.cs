using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public virtual Booking Booking { get; set; } = null!;

        public decimal Amount { get; set; }
        public decimal CashGiven { get; set; }
        public decimal ChangeReturned { get; set; }

        [StringLength(30)]
        public string PaymentMethod { get; set; } = PaymentMethods.Cash;

        [StringLength(20)]
        public string Status { get; set; } = Models.PaymentStatus.Success;

        public DateTime PaymentDate { get; set; }
    }
}
