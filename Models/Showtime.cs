using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Showtime
    {
        [Key]
        public int ShowtimeId { get; set; }

        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; } 

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; } 

        public DateTime StartTime { get; set; } 

        public DateTime EndTime { get; set; } 

        public decimal TicketPrice { get; set; }
        
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        
        public virtual ICollection<SeatHold> SeatHolds { get; set; } = new List<SeatHold>();
    }
}