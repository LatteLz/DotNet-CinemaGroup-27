using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class SeatHold
    {
        [Key]
        public int HoldId { get; set; }

        public int ShowtimeId { get; set; }

        [ForeignKey("ShowtimeId")]
        public virtual Showtime Showtime { get; set; } // Thuộc tính điều hướng liên kết với Showtime

        public int SeatId { get; set; }

        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set; } // Thuộc tính điều hướng liên kết với Seat

        public DateTime ExpiresAt { get; set; } // Thời gian hết hạn giữ ghế (ví dụ: +3 phút)

        [StringLength(100)]
        public string SessionId { get; set; } // Mã phiên làm việc/thiết bị đang giữ ghế
    }
}