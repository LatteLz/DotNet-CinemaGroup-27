using System.Collections.Generic;
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
        [StringLength(20)]
        public string SeatNumber { get; set; } 

        [Required]
        [StringLength(50)]
        public string SeatType { get; set; } 

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } 
        
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        
        public virtual ICollection<SeatHold> SeatHolds { get; set; } = new List<SeatHold>();
    }
}