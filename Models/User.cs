using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required] [StringLength(50)] public string Username { get; set; } = string.Empty;

    [Required] [StringLength(255)] public string PasswordHash { get; set; } = string.Empty;

    [Required] [StringLength(100)] public string Fullname { get; set; } =string.Empty;

     [StringLength(50)] public string Role { get; set; } = "Staff"; //ad,staff, khach
    public bool IsActive  { get; set; } = true;
    public DateTime CreateAt{ get; set; }= DateTime.Now;

}