using CinemaManagement.Models;

namespace CinemaManagement.Helpers
{
    public static class AppSession
    {
        public static User? CurrentUser { get; private set; }

        public static bool IsLoggedIn => CurrentUser != null;
        public static bool IsAdmin => CurrentUser?.Role == Roles.Admin;
        public static string DisplayName => CurrentUser?.Fullname ?? "Khách";

        public static void SignIn(User user) => CurrentUser = user;

        public static void SignOut() => CurrentUser = null;

        public static void RequireAdmin(Form form) => Guard(form, () => IsAdmin,
            "Chức năng này chỉ dành cho tài khoản Quản trị (Admin).");

        public static void RequireLogin(Form form) => Guard(form, () => IsLoggedIn,
            "Vui lòng đăng nhập để sử dụng chức năng này.");

        private static void Guard(Form form, System.Func<bool> allowed, string message)
        {
            form.Load += (_, _) =>
            {
                if (allowed()) return;

                MessageBox.Show(message, "Không có quyền truy cập", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                form.BeginInvoke(new MethodInvoker(form.Close));
            };
        }
    }
}
