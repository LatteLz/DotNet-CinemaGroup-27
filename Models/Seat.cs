using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }
        public int RoomId { get; set; }
        public string SeatNumber { get; set; } // A1, A2...
        public string SeatType { get; set; }   // Standard, Couple
    }
}