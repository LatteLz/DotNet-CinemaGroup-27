using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class ComboItem
    {
        [Key]
        public int ComboId { get; set; }
        public string ComboName { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
    }
}