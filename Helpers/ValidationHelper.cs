using System.Text.RegularExpressions;

namespace CinemaManagement.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string pattern = @"^(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})$";
            return Regex.IsMatch(phone, phone.Trim());
        }

        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return false;

            return true;
        }

        public static bool IsNotEmpty(params string[] values)
        {
            foreach (var val in values)
            {
                if (string.IsNullOrWhiteSpace(val))
                    return false;
            }
            return true;
        }
    }
}