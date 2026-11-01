using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CinemaManagement.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }
        public int ShowtimeId { get; set; }
        [ForeignKey("ShowtimeId")]
        public virtual Showtime Showtime { get; set;}
        public int SeatId { get; set; }
        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set;}
        public decimal Price { get; set; }
    }
}