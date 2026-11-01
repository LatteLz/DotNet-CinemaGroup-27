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

        public int Duration { get; set; } 

        [StringLength(100)]
        public string Genre { get; set; } 

        [StringLength(20)]
        public string AgeRating { get; set; } 

        [StringLength(255)]
        public string PosterPath { get; set; } 

        public string Description { get; set; } 
        
        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}