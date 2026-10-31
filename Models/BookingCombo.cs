using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class BookingCombo
    {
        [Key]
        public int BookingComboId { get; set; }
        public int BookingId { get; set; }
        public int ComboId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}