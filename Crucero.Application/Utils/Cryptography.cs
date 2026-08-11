using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Utils
{
    internal class Cryptography
    {
        private const string Pbkdf2Prefix = "PBKDF2";
        private const int Pbkdf2Iterations = 100_000;
        private const int SaltSize = 16;
        private const int KeySize = 32;

        public static string Encrypt(string texto, string secret)
        {
            if (string.IsNullOrWhiteSpace(secret) || secret.Length < 32)
            {
                throw new ArgumentException("Crypto secret must have at least 32 characters.", nameof(secret));
            }

            byte[] plainBytes = Encoding.UTF8.GetBytes(texto);
            string hash = ComputeHash(secret.Substring(0, 32));
            byte[] key = Encoding.UTF8.GetBytes(hash); // 32 bytes        
            byte[] iv = [33, 24, 31, 46, 75, 64, 97, 18, 89, 10, 111, 132, 131, 144, 145, 250]; //16 bytes
            byte[] encryptedBytes;

            // Set up the encryption objects
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Encrypt the input plaintext using the AES algorithm
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                }
            }
            //return string encrypt
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Pbkdf2Iterations,
                HashAlgorithmName.SHA256,
                KeySize);

            return $"{Pbkdf2Prefix}${Pbkdf2Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public static bool IsPbkdf2Hash(string storedPassword)
        {
            return !string.IsNullOrWhiteSpace(storedPassword) && storedPassword.StartsWith($"{Pbkdf2Prefix}$", StringComparison.Ordinal);
        }

        public static bool VerifyPassword(string plainPassword, string storedPassword, string secret)
        {
            if (string.IsNullOrWhiteSpace(storedPassword))
            {
                return false;
            }

            if (IsPbkdf2Hash(storedPassword))
            {
                return VerifyPbkdf2Password(plainPassword, storedPassword);
            }

            if (string.IsNullOrWhiteSpace(secret) || secret.Length < 32)
            {
                return false;
            }

            // Backward compatibility for existing encrypted passwords.
            var legacyEncrypted = Encrypt(plainPassword, secret);
            return string.Equals(legacyEncrypted, storedPassword, StringComparison.Ordinal);
        }

        private static bool VerifyPbkdf2Password(string plainPassword, string storedPassword)
        {
            string[] parts = storedPassword.Split('$');
            if (parts.Length != 4)
            {
                return false;
            }

            if (!int.TryParse(parts[1], out int iterations))
            {
                return false;
            }

            byte[] salt;
            byte[] expectedHash;
            try
            {
                salt = Convert.FromBase64String(parts[2]);
                expectedHash = Convert.FromBase64String(parts[3]);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                plainPassword,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }

        private static string ComputeHash(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder();
                foreach (var c in data)
                {
                    sb.Append(c.ToString("x2"));
                }
                return sb.ToString();
            }
        }

    }
}