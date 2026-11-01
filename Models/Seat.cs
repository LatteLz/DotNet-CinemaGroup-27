using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        [Required]
        [StringLength(10)]
        public string SeatNumber { get; set; } 
        [Required]
        [StringLength(20)]
        public string SeatType { get; set; } = "Standard";
        
    }
}