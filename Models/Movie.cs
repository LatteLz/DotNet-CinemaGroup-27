using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public int Duration { get; set; } // Thời lượng phim (phút)

        [StringLength(100)]
        public string Genre { get; set; } // Thể loại phim

        [StringLength(20)]
        public string AgeRating { get; set; } // Giới hạn độ tuổi (P, T13, T16, T18...)

        [StringLength(255)]
        public string PosterPath { get; set; } // Đường dẫn ảnh poster

        public string Description { get; set; } // Mô tả nội dung phim

        // Thuộc tính điều hướng liên kết với các suất chiếu của phim này
        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}