using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class BookingCombo
    {
        [Key]
        public int BookingComboId { get; set; }

        public int BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public virtual Booking Booking { get; set; } = null!;

        public int ComboId { get; set; }

        [ForeignKey(nameof(ComboId))]
        public virtual ComboItem ComboItem { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
