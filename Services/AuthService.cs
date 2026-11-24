using CinemaManagement.Data;
using CinemaManagement.Helpers;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class AuthService
    {
        public User? Login(string username, string password, out string errorMessage)
        {
            errorMessage = string.Empty;
            using var db = DbFactory.Create();

            string name = (username ?? string.Empty).Trim().ToLower();
            var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Username.ToLower() == name);

            if (user == null || !PasswordHelper.VerifyPassword(password ?? string.Empty, user.PasswordHash, user.Salt))
            {
                errorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác!";
                return null;
            }

            if (!user.IsActive)
            {
                errorMessage = "Tài khoản này đã bị khóa. Vui lòng liên hệ quản trị viên.";
                return null;
            }

            return user;
        }

        public List<User> GetAllUsers()
        {
            using var db = DbFactory.Create();
            return db.Users.AsNoTracking().OrderBy(u => u.Id).ToList();
        }

        public User? GetById(int userId)
        {
            using var db = DbFactory.Create();
            return db.Users.AsNoTracking().FirstOrDefault(u => u.Id == userId);
        }

        public void AddUser(string username, string password, string fullname, string? email, string? phone, string role, bool isActive)
        {
            username = (username ?? string.Empty).Trim();
            ValidateProfile(username, fullname, email, phone, role);
            if (!ValidationHelper.IsStrongPassword(password))
                throw new BusinessException("Mật khẩu phải có ít nhất 6 ký tự.");

            using var db = DbFactory.Create();
            string lower = username.ToLower();
            if (db.Users.Any(u => u.Username.ToLower() == lower))
                throw new BusinessException($"Tên đăng nhập '{username}' đã tồn tại.");

            PasswordHelper.CreatePasswordHash(password, out string hash, out string salt);
            db.Users.Add(new User
            {
                Username = username,
                PasswordHash = hash,
                Salt = salt,
                Fullname = fullname.Trim(),
                Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim(),
                Role = role,
                IsActive = isActive,
                CreateAt = DateTime.Now
            });
            db.SaveChanges();
        }

        public void UpdateUser(int userId, string fullname, string? email, string? phone, string role, bool isActive)
        {
            using var db = DbFactory.Create();
            var user = db.Users.Find(userId) ?? throw new BusinessException("Không tìm thấy tài khoản.");
            ValidateProfile(user.Username, fullname, email, phone, role);

            bool losesAdmin = user.Role == Roles.Admin && user.IsActive && (role != Roles.Admin || !isActive);
            if (losesAdmin)
            {
                if (AppSession.CurrentUser?.Id == userId)
                    throw new BusinessException("Bạn không thể tự khóa hoặc tự hạ quyền tài khoản đang đăng nhập.");
                EnsureAnotherActiveAdmin(db, userId);
            }

            user.Fullname = fullname.Trim();
            user.Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
            user.Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
            user.Role = role;
            user.IsActive = isActive;
            db.SaveChanges();
        }

        public void ResetPassword(int userId, string newPassword)
        {
            if (!ValidationHelper.IsStrongPassword(newPassword))
                throw new BusinessException("Mật khẩu phải có ít nhất 6 ký tự.");

            using var db = DbFactory.Create();
            var user = db.Users.Find(userId) ?? throw new BusinessException("Không tìm thấy tài khoản.");
            PasswordHelper.CreatePasswordHash(newPassword, out string hash, out string salt);
            user.PasswordHash = hash;
            user.Salt = salt;
            db.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            if (AppSession.CurrentUser?.Id == userId)
                throw new BusinessException("Bạn không thể xóa tài khoản đang đăng nhập.");

            using var db = DbFactory.Create();
            var user = db.Users.Find(userId) ?? throw new BusinessException("Không tìm thấy tài khoản.");
            if (user.Role == Roles.Admin && user.IsActive)
                EnsureAnotherActiveAdmin(db, userId);

            if (db.Bookings.Any(bk => bk.CreatedByUserId == userId))
                throw new BusinessException("Tài khoản này đã lập hóa đơn bán vé nên không thể xóa (để giữ lịch sử giao dịch). Hãy chuyển trạng thái sang 'Tạm khóa'.");

            db.Users.Remove(user);
            db.SaveChanges();
        }

        private static void EnsureAnotherActiveAdmin(CinemaDbContext db, int exceptUserId)
        {
            bool hasOther = db.Users.Any(u => u.Id != exceptUserId && u.Role == Roles.Admin && u.IsActive);
            if (!hasOther)
                throw new BusinessException("Hệ thống phải còn ít nhất một tài khoản Admin đang hoạt động.");
        }

        private static void ValidateProfile(string username, string fullname, string? email, string? phone, string role)
        {
            if (!ValidationHelper.IsValidUsername(username))
                throw new BusinessException("Tên đăng nhập 3-50 ký tự, chỉ gồm chữ, số và . _ - @");
            if (string.IsNullOrWhiteSpace(fullname))
                throw new BusinessException("Họ tên không được để trống.");
            if (!string.IsNullOrWhiteSpace(email) && !ValidationHelper.IsValidEmail(email))
                throw new BusinessException("Email không đúng định dạng.");
            if (!string.IsNullOrWhiteSpace(phone) && !ValidationHelper.IsValidPhoneNumber(phone))
                throw new BusinessException("Số điện thoại không hợp lệ (10 số, bắt đầu 03/05/07/08/09).");
            if (role != Roles.Admin && role != Roles.Staff)
                throw new BusinessException("Vai trò chỉ có thể là Admin hoặc Staff.");
        }
    }
}
