using SuaveKeys.Core.Data.Providers;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SuaveKeys.Infrastructure.Data.Providers
{
    /// <summary>
    /// Secure hash provider. Based off of asp.net core identity v3 hash tools.
    /// This is because we don't want to re-invent the wheel, but also don't want full identity
    /// since it restrains our code re-usability
    /// </summary>
    public class SecureHashProvider : IHashProvider
    {
        private const int SALT_BYTE_SIZE = 24;
        private const int HASH_BYTE_SIZE = 24;
        private const int PBKDF2_ITERATIONS = 1000;

        private const int ITERATION_INDEX = 0;
        private const int SALT_INDEX = 1;
        private const int PBKDF2_INDEX = 2;

        /// <summary>
        /// Hashs the password.
        /// </summary>
        /// <returns>The password.</returns>
        /// <param name="password">Password.</param>
        public string HashPassword(string password)
        {
            return CreateComplexHash(password);
        }
        /// <summary>
        /// Gets if the given password matches the hashed password
        /// </summary>
        /// <returns><c>true</c>, if password match was gotten, <c>false</c> otherwise.</returns>
        /// <param name="hashedPassword">Hashed password.</param>
        /// <param name="password">Password.</param>
        public bool GetPasswordMatch(string hashedPassword, string password)
        {
            if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(password)) return false;
            char[] delimiter = { ':' };
            string[] split = hashedPassword.Split(delimiter);
            int iterations = int.Parse(split[ITERATION_INDEX]);
            byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        /// <summary>
        /// Calcualte byte array equals with slow check
        /// </summary>
        /// <returns><c>true</c>, if equals was slowed, <c>false</c> otherwise.</returns>
        /// <param name="a">The alpha component.</param>
        /// <param name="b">The blue component.</param>
        private bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        /// <summary>
        /// Creates hash with 1000 iterations using pbkdf2
        /// </summary>
        /// <returns>The complex hash.</returns>
        /// <param name="password">Password.</param>
        private string CreateComplexHash(string password)
        {
            var csprng = RandomNumberGenerator.Create();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
            return PBKDF2_ITERATIONS + ":" +
                Convert.ToBase64String(salt) + ":" +
                Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Get the pbkdf2 hash with salt
        /// </summary>
        /// <returns>The PBKDF.</returns>
        /// <param name="password">Password.</param>
        /// <param name="salt">Salt.</param>
        /// <param name="iterations">Iterations.</param>
        /// <param name="outputBytes">Output bytes.</param>
        private byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }

    }
}
