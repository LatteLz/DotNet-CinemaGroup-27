using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace CinemaManagement.Models
{
    public class ComboItem
    {
        [Key]
        public int ComboId { get; set; }
        [Required]
        [StringLength(100)]
        public string ComboName { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<BookingCombo> BookingCombos{get;set;}=new List<BookingCombo>();
        
        
    }
}