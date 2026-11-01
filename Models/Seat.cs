using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; } // Thuộc tính điều hướng liên kết với Room

        [Required]
        [StringLength(20)]
        public string SeatNumber { get; set; } // Số ghế (Ví dụ: A1, A2...)

        [Required]
        [StringLength(50)]
        public string SeatType { get; set; } // Phân loại: Standard, VIP, Sweetbox...

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } // Giá tiền mặc định của ghế theo hạng

        // Thuộc tính điều hướng liên kết với vé
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        // Thuộc tính điều hướng liên kết với cơ chế giữ ghế tạm thời
        public virtual ICollection<SeatHold> SeatHolds { get; set; } = new List<SeatHold>();
    }
}