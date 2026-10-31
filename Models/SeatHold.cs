using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class SeatHold
    {
        [Key]
        public int HoldId { get; set; }
        public int ShowtimeId { get; set; }
        public int SeatId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string SessionId { get; set; }
    }
}