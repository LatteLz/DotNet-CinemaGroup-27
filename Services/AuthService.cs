using System;
using System.Collections.Generic;
using System.Linq;
using CinemaManagement.Data;
using CinemaManagement.Models;
using CinemaManagement.Helpers;

namespace CinemaManagement.Services
{
    public class AuthService
    {
        public User Login(string username, string password)
        {
            using var context = new CinemaDbContext();
    
            var user = context.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                bool isValid = PasswordHelper.VerifyPassword(password, user.PasswordHash, user.Salt);
                if (isValid)
                {
                    return user;
                }
            }
            return null;
        }

       
        public List<User> GetAllUsers()
        {
            using var context = new CinemaDbContext();
            return context.Users.ToList();
        }

       
        public bool AddUser(string username, string password, string role)
        {
            try
            {
                using var context = new CinemaDbContext();

                PasswordHelper.CreatePasswordHash(password, out string passwordHash, out string salt);

                var newUser = new User
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    Salt = salt,
                    Role = role
                };

                context.Users.Add(newUser);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm tài khoản: " + ex.Message);
            }
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                using var context = new CinemaDbContext();
                var user = context.Users.Find(userId);
                if (user != null)
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa tài khoản: " + ex.Message);
            }
        }
    }
}