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
        public string ComboName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        [StringLength(255)]
        public string? ImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<BookingCombo> BookingCombos { get; set; } = new List<BookingCombo>();
    }
}
