using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        [StringLength(100)]
        public string RoomName { get; set; } = string.Empty;

        [StringLength(50)]
        public string RoomType { get; set; } = "Standard 2D";

        public int RowCount { get; set; }

        public int ColCount { get; set; }

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}
