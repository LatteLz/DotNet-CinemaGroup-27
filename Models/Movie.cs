using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
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
        [StringLength(50)]
        public string Genre { get; set; }
        [StringLength(10)]
        public string AgeRating { get; set; }
        public string PosterPath { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Showtime> Showtimes {get;set;} = new List<Showtime>();
        
    }
}