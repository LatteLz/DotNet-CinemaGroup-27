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

            string adminEmail = "admin.director@cineluxe.vn";
            var adminUser = context.Users.FirstOrDefault(u => u.Username == adminEmail);

            if (adminUser == null)
            {PasswordHelper.CreatePasswordHash("admin123", out string hash, out string salt);

                context.Users.Add(new User
                {
                    Username = adminEmail,
                    Fullname = "Quản Trị Viên",
                    Role = "Admin",
                    PasswordHash = hash,
                    Salt = salt,
                    IsActive = true,
                    CreateAt = DateTime.Now
                });
                context.SaveChanges();
            }
        }
    }
}