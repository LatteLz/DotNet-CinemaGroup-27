using System;
using System.Linq;
using CinemaManagement.Models;
using CinemaManagement.Helpers;

namespace CinemaManagement.Data
{
public static class DbInitializer
{
    public static void Initialize(CinemaDbContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            PasswordHelper.CreatePasswordHash("admin123", out string adminHash, out string adminSalt);
            PasswordHelper.CreatePasswordHash("admin123", out string staffHash, out string staffSalt);

            context.Users.AddRange(
                new User
                {
                    Username = "admin.director@cineluxe.vn",
                    Fullname = "Quản Trị Viên",
                    Role = "Admin",
                    PasswordHash = adminHash,
                    Salt = adminSalt,
                    IsActive = true,
                    CreateAt = DateTime.Now
                },
                new User
                {
                    Username = "staff.pos@cineluxe.vn",
                    Fullname = "Thảo Hân (Thu Ngân)",
                    Role = "Staff",
                    PasswordHash = staffHash,
                    Salt = staffSalt,
                    IsActive = true,
                    CreateAt = DateTime.Now
                }
            );
            context.SaveChanges();
        }
    }
}
}