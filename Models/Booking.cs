using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CinemaManagement.Models
{
    public class Booking
    {
        [Key] public int BookingId { get; set; }
        public DateTime BookingTime { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)]
        
        public string Status{get;set;}="Pending";
        public int ShowtimeId{get;set;}
        [ForeignKey("ShowtimeId")]
        public virtual Showtime Showtime {get;set;} 
        public virtual ICollection<Ticket> Tickets {get;set;}=new List<Ticket>();
        
    }
    }
    
    
    
    