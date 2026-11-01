using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        [StringLength(100)]
        public string RoomName { get; set; } // Tên phòng chiếu (Ví dụ: Phòng 1, Phòng 2...)

        public int RowCount { get; set; } // Số hàng ghế trong ma trận phòng

        public int ColCount { get; set; } // Số cột ghế trong ma trận phòng

        // Thuộc tính điều hướng liên kết với danh sách ghế ngồi trong phòng
        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

        // Thuộc tính điều hướng liên kết với các suất chiếu diễn ra tại phòng này
        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}