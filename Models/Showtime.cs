using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Showtime
    {
        [Key]
        public int ShowtimeId { get; set; }
        public int MovieId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal TicketPrice { get; set; } 
    }
}