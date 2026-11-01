using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        [StringLength(100)]
        public string RoomName { get; set; } 

        public int RowCount { get; set; } 

        public int ColCount { get; set; } 
        
        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
        
        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}