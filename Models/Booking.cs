using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [StringLength(30)]
        public string BookingCode { get; set; } = string.Empty;

        public int? ShowtimeId { get; set; }

        [ForeignKey(nameof(ShowtimeId))]
        public virtual Showtime? Showtime { get; set; }

        [StringLength(20)]
        public string Channel { get; set; } = SalesChannels.Kiosk;

        public int? CreatedByUserId { get; set; }

        [StringLength(200)]
        public string MovieTitle { get; set; } = string.Empty;

        [StringLength(200)]
        public string ShowtimeInfo { get; set; } = string.Empty;

        public decimal TicketAmount { get; set; }
        public decimal ComboAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public virtual ICollection<BookingCombo> BookingCombos { get; set; } = new List<BookingCombo>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
