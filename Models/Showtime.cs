using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Showtime
    {
        [Key]
        public int ShowtimeId { get; set; }

        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; } // Thuộc tính điều hướng liên kết với Movie

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; } // Thuộc tính điều hướng liên kết với Room

        public DateTime StartTime { get; set; } // Thời gian bắt đầu suất chiếu

        public DateTime EndTime { get; set; } // Thời gian kết thúc suất chiếu

        [Column(TypeName = "decimal(18,2)")]
        public decimal TicketPrice { get; set; } // Giá vé cơ bản của suất chiếu

        // Thuộc tính điều hướng liên kết với danh sách vé đã bán trong suất chiếu này
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        // Thuộc tính điều hướng liên kết với danh sách giữ ghế tạm thời
        public virtual ICollection<SeatHold> SeatHolds { get; set; } = new List<SeatHold>();
    }
}