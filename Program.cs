using System;
using System.Security.Cryptography;
using System.Text;
using CinemaManagement.Data;
using CinemaManagement.Forms.Admin;
using CinemaManagement.Models;

namespace CinemaManagement
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            using (var context = new CinemaDbContext())
            {
                DbInitializer.Initialize(context);

                string targetEmail = "admin.director@cineluxe.vn";
                var existingUser = context.Users.FirstOrDefault(u => u.Username == targetEmail);
                
                string rawPassword = "admin123";
                string salt = "Sqe0/uWJYrNM0pFYYo0Z=";

                using (var sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawPassword + salt));
                    string hash = Convert.ToBase64String(hashBytes);

                    if (existingUser == null)
                    {
                        context.Users.Add(new User
                        {
                            Username = targetEmail,
                            Fullname = "Quản Trị Viên",
                            Role = "Admin",
                            PasswordHash = hash,
                            Salt = salt,
                            IsActive = true,
                            CreateAt = DateTime.Now
                        });
                    }
                    else
                    {
                        existingUser.PasswordHash = hash;
                        existingUser.Salt = salt;
                    }
                    context.SaveChanges();
                }
            }

            Application.Run(new FrmRoleSelection());
        }
    }
}