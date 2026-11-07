using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string ShowtimeInfo { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
    
    
    