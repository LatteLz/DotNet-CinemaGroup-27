using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Fullname { get; set; } = string.Empty;

        [StringLength(50)]
        public string Role { get; set; } = "Staff";

        public string Salt { get; set; } 

        public bool IsActive { get; set; } = true;
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}