using System;
using System.Security.Cryptography;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Utility class for encryption and decryption of data.
    /// </summary>
    public static class EncryptionUtility
    {

        // Disable the warning.
#pragma warning disable SYSLIB0021
        private static readonly TripleDESCryptoServiceProvider _transform;

        static EncryptionUtility()
        {
#pragma warning disable CA5350
            _transform = new TripleDESCryptoServiceProvider();

            //24 byte string size
            _transform.Key = System.Text.Encoding.ASCII.GetBytes("XSPOC123456789Encryption");
            //8 byte initialization vector
            _transform.IV = System.Text.Encoding.ASCII.GetBytes("1Initial");
        }

        /// <summary>
        /// Decrypts an encrypted string
        /// </summary>
        /// <param name="encryptedText">The encrypted string</param>
        /// <returns>a plaintext decrypted string</returns>
        public static string Decrypt(string encryptedText)
        {
            if (encryptedText == null)
            {
                throw new ArgumentNullException(nameof(encryptedText));
            }

            var memoryStream = new System.IO.MemoryStream();

            var encryptedBytes = Convert.FromBase64String(encryptedText);
            var decryptedStream = new CryptoStream(memoryStream, transform: _transform.CreateDecryptor(),
                mode: CryptoStreamMode.Write);

            decryptedStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            decryptedStream.FlushFinalBlock();

            var plainText = System.Text.Encoding.Unicode.GetString(memoryStream.ToArray());

            return plainText;
        }

#pragma warning restore CA5350
#pragma warning restore SYSLIB0021

    }
}
