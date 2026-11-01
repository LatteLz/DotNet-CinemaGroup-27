using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class ComboItem
    {
        [Key]
        public int ComboId { get; set; }

        [Required]
        [StringLength(100)]
        public string ComboName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } 

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; } 

        public int MinInventory { get; set; } 

        [StringLength(50)]
        public string Category { get; set; } 

        [StringLength(50)]
        public string Channel { get; set; } 

        [StringLength(255)]
        public string ImagePath { get; set; } 
        
        public virtual ICollection<BookingCombo> BookingCombos { get; set; } = new List<BookingCombo>();
    }
}