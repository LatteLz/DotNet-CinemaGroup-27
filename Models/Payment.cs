using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public virtual Booking Booking{get;set;}
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; }=DateTime.Now;
        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Completed";
    }
}