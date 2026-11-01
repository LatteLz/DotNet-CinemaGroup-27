using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        [Required]
        [StringLength(50)]
        public string RoomName { get; set; }
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public virtual ICollection<Showtime> Showtimes{get;set;}=new List<Showtime>();
    }
}