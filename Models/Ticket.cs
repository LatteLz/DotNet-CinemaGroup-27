using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        [StringLength(40)]
        public string TicketCode { get; set; } = string.Empty;

        public int BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public virtual Booking Booking { get; set; } = null!;

        public int ShowtimeId { get; set; }

        [ForeignKey(nameof(ShowtimeId))]
        public virtual Showtime Showtime { get; set; } = null!;

        public int SeatId { get; set; }

        [ForeignKey(nameof(SeatId))]
        public virtual Seat Seat { get; set; } = null!;

        public decimal Price { get; set; }

        [Required]
        [StringLength(20)]
        public string AudienceType { get; set; } = AudienceTypes.Adult;
    }
}
