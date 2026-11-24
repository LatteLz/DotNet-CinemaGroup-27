using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class SeatHold
    {
        [Key]
        public int HoldId { get; set; }

        public int ShowtimeId { get; set; }

        [ForeignKey(nameof(ShowtimeId))]
        public virtual Showtime Showtime { get; set; } = null!;

        public int SeatId { get; set; }

        [ForeignKey(nameof(SeatId))]
        public virtual Seat Seat { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        [Required]
        [StringLength(100)]
        public string SessionId { get; set; } = string.Empty;
    }
}
