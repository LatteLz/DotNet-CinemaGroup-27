using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    [Table("ComboItems")]
    public class ComboItem
    {
        [Key]
        public int ComboId { get; set; }

        [Required]
        [StringLength(100)]
        public string ComboName { get; set; }

        public decimal Price { get; set; }

        public string ImagePath { get; set; }

        public virtual ICollection<BookingCombo> BookingCombos { get; set; } = new List<BookingCombo>();
    }
}