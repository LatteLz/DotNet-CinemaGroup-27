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
        public virtual Booking Booking { get; set; } // Thuộc tính điều hướng liên kết với Booking

        public int ShowtimeId { get; set; }

        [ForeignKey("ShowtimeId")]
        public virtual Showtime Showtime { get; set; } // Thuộc tính điều hướng liên kết với Showtime

        public int SeatId { get; set; }

        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set; } // Thuộc tính điều hướng liên kết với Seat

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Giá vé thực tế của ghế khi bán
    }
}