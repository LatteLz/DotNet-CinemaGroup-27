using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class ComboItem
    {
        [Key]
        public int ComboId { get; set; }

        [Required]
        [StringLength(100)]
        public string ComboName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Giá niêm yết

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; } // Giá vốn (phục vụ báo cáo tài chính)

        public int MinInventory { get; set; } // Cảnh báo tồn kho tối thiểu

        [StringLength(50)]
        public string Category { get; set; } // Phân loại (Bắp, Nước, Combo...)

        [StringLength(50)]
        public string Channel { get; set; } // Kênh hiển thị (POS, Kiosk, All)

        [StringLength(255)]
        public string ImagePath { get; set; } // Đường dẫn ảnh đại diện món

        // Thuộc tính điều hướng liên kết với chi tiết booking combo
        public virtual ICollection<BookingCombo> BookingCombos { get; set; } = new List<BookingCombo>();
    }
}