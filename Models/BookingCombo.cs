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
        public virtual Booking Booking { get; set; }

        public int ComboId { get; set; } // Sửa lại chữ d thành hoa (ComboId) để đồng bộ đặt tên
        
        [ForeignKey("ComboId")]
        public virtual ComboItem ComboItem { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }
}