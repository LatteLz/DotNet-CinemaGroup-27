using System.Globalization;
using System.Text.RegularExpressions;

namespace CinemaManagement.Helpers
{
    public static class ValidationHelper
    {
        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex PhoneRegex = new(@"^0[35789]\d{8}$", RegexOptions.Compiled);

        private static readonly Regex UsernameRegex = new(@"^[A-Za-z0-9._@\-]{3,50}$", RegexOptions.Compiled);

        public static bool IsValidEmail(string? email)
            => !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email.Trim());

        public static bool IsValidPhoneNumber(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            string digits = phone.Replace(" ", "").Replace(".", "").Replace("-", "");
            return PhoneRegex.IsMatch(digits);
        }

        public static bool IsValidUsername(string? username)
            => !string.IsNullOrWhiteSpace(username) && UsernameRegex.IsMatch(username.Trim());

        public static bool IsStrongPassword(string? password)
            => !string.IsNullOrWhiteSpace(password) && password.Length >= 6;

        public static bool IsNotEmpty(params string?[] values)
        {
            foreach (var val in values)
            {
                if (string.IsNullOrWhiteSpace(val))
                    return false;
            }
            return true;
        }

        public static bool TryParseMoney(string? text, out decimal value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(text)) return false;

            string clean = text.Replace("VNĐ", "").Replace("đ", "").Replace("Đ", "")
                               .Replace(".", "").Replace(",", "").Replace(" ", "").Trim();
            return decimal.TryParse(clean, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }

        public static bool IsPositiveMoney(string? text, out decimal value)
            => TryParseMoney(text, out value) && value > 0;
    }
}
