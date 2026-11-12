using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public int TotalSeats 
        { 
            get => RowCount * ColCount > 0 ? RowCount * ColCount : 50; 
            set { } 
        }
        
        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
        
        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}