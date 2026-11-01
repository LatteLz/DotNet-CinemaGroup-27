using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public DateTime BookingTime { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)]")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // Ví dụ: Pending, Completed, Cancelled

        // Thuộc tính điều hướng (Navigation Properties) liên kết với Vé, Combo và Thanh toán
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        
        public virtual ICollection<BookingCombo> BookingCombos { get; set; } = new List<BookingCombo>();
        
        public virtual Payment Payment { get; set; }
    }
}