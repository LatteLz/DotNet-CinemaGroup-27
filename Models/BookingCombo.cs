using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CinemaManagement.Models
{
    public class BookingCombo
    {
        [Key]
        public int BookingComboId { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public virtual Booking Booking {get;set;}
        public int ComboId { get; set; }
        [ForeignKey("ComboId")]
        public virtual Combo Combo {get;set;}
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}