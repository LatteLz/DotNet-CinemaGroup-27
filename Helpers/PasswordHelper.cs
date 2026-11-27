namespace CinemaManagement.Helpers
{
    public static class PasswordHelper
    {
        public static void CreatePasswordHash(string password, out string passwordHash, out string salt)
        {
            passwordHash = password;
            salt = string.Empty;
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            return password == storedHash;
        }
    }
}
