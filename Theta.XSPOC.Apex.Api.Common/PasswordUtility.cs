using System;
using System.Security.Cryptography;
using TextEncoding = System.Text.Encoding;

namespace Theta.XSPOC.Apex.Api.Common
{
    /// <summary>
    /// Hashes and verifies passwords
    /// </summary>
    public static class PasswordUtility
    {

        #region Constants

        private const string SALT_KEY = @"pQIOZdJ7J/t/hNQJT8jn5A==";
        private const int SALT_SIZE = 16;
        private const int ITERATION_COUNT = 1000;
        private const int BYTE_COUNT = 36;

        private static readonly TextEncoding ENCODING = TextEncoding.Unicode;

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates a hash for a specified password using a specified username for the salt
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>A 48-character hash for the specified username and password</returns>
        /// <exception cref="ArgumentNullException">
        /// username is null
        /// OR
        /// password is null
        /// </exception>
        public static string HashPassword(string username, string password)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            byte[] salt = GenerateSalt(username);

#pragma warning disable CA5379
            Rfc2898DeriveBytes hasher = new Rfc2898DeriveBytes(password, salt, ITERATION_COUNT);
#pragma warning restore CA5379

            byte[] hash = hasher.GetBytes(BYTE_COUNT);

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Verifies that a specified username and password results in a specified hash
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="hash">The expected hash</param>
        /// <returns>True if the generated hash equals the specified hash; otherwise, false</returns>
        /// <exception cref="ArgumentNullException">
        /// username is null
        /// OR
        /// password is null
        /// OR
        /// hash is null
        /// </exception>
        public static bool VerifyPassword(string username, string password, string hash)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (hash == null)
            {
                throw new ArgumentNullException(nameof(hash));
            }

            string actualHash = HashPassword(username, password);

            return hash.Equals(actualHash, StringComparison.Ordinal);
        }

        #endregion

        #region Private Methods

        private static byte[] GenerateSalt(string username)
        {
            byte[] salt = ENCODING.GetBytes(SALT_KEY);

#pragma warning disable CA5379
            Rfc2898DeriveBytes hasher = new Rfc2898DeriveBytes(username.ToLower(), salt, ITERATION_COUNT);
#pragma warning restore CA5379

            return hasher.GetBytes(SALT_SIZE);
        }

        #endregion

    }
}
