using System;
using System.Security.Cryptography;
using System.Text;

namespace CinemaManagement.Helpers
{
    public static class PasswordHelper
    {
        public static void CreatePasswordHash(string password, out string passwordHash, out string salt)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);
            
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                password,
                saltBytes,
                iterations: 100000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32
            );
            passwordHash = Convert.ToBase64String(hashBytes);
        }
        
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                password,
                saltBytes,
                iterations: 100000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32
            );
            string computedHash = Convert.ToBase64String(hashBytes);
            return computedHash == storedHash;
        }
    }
}