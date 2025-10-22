using System.Security.Cryptography;

namespace SClarkC971PA.Services
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;         // 128-bit
        private const int KeySize = 32;         // 256-bit
        private const int Iterations = 120_000; //High number protects against brute-force attacks

        // Encodes algo+params so we can store a single string in the DB:
        // PBKDF2$<iterations>$<saltBase64>$<hashBase64>
        public static string HashPassword(string password)
        {   //Create a new salt
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            //Fill the salt with cryptographically strong random bytes
            rng.GetBytes(salt);
            //Derive the key using PBKDF2 with the specified parameters
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            //Get the derived key
            var key = pbkdf2.GetBytes(KeySize);
            //Return the encoded hash string
            return $"PBKDF2${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(key)}";
        }

        public static bool VerifyPassword(string password, string encodedHash)
        {   //Split the encoded hash into its components
            var parts = encodedHash.Split('$', StringSplitOptions.RemoveEmptyEntries);
            //Check if the format is correct
            if (parts.Length != 4 || parts[0] != "PBKDF2") return false;
            //Extract parameters
            var iterations = int.Parse(parts[1]);
            var salt = Convert.FromBase64String(parts[2]);
            var expectedKey = Convert.FromBase64String(parts[3]);
            //Derive the key from the provided password
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            //Get the derived key
            var actualKey = pbkdf2.GetBytes(expectedKey.Length);
            //Compare the keys in constant time
            return CryptographicOperations.FixedTimeEquals(actualKey, expectedKey);
        }
    }
}
