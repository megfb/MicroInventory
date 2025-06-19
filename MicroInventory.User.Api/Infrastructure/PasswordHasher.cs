using System.Security.Cryptography;
using MicroInventory.User.Api.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MicroInventory.User.Api.Infrastructure
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 128 / 8; // 128 bits = 16 bytes
        private const int KeySize = 256 / 8; // 256 bits = 32 bytes
        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: KeySize
                );

            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hashed);
        }

        public bool Verify(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
                throw new FormatException("The hashed password format is invalid.");
            
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);
            byte[] actualHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: expectedHash.Length
            );
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}
