using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Genre { get; set; }
        public string AgeRating { get; set; }
        public string PosterPath { get; set; }
        public string Description { get; set; }
    }
}