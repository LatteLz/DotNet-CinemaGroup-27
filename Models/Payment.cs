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
        public virtual Booking Booking { get; set; } // Thuộc tính điều hướng liên kết với Booking

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // Số tiền thanh toán

        public DateTime PaymentTime { get; set; } = DateTime.Now; // Thời gian giao dịch

        [StringLength(50)]
        public string PaymentMethod { get: set; } // Phương thức: Cash, Momo, Banking...

        [StringLength(50)]
        public string Status { get; set; } // Trạng thái: Thành công, Thất bại, Đang chờ
    }
}