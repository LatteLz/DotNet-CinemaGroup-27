using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Salt { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Fullname { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(50)]
        public string Role { get; set; } = Roles.Staff;

        public bool IsActive { get; set; } = true;
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
