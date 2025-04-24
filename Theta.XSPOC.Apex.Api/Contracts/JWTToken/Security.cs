using System.Security.Cryptography;
using System.Text;
using System;

namespace Theta.XSPOC.Apex.Api.Contracts.JWTToken
{
    /// <summary>
    /// Represents the SecurityDecript.
    /// </summary>
    public static class Security
    {
        private static string Key => "D0vER@_" + DateTime.UtcNow.ToUniversalTime().ToString("dd_MM_yy") + "@";

        /// <summary>
        /// Decript the reference token that got genareated from Azure AD.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>string.</returns>
        public static string Decrypt(string input)
        {
            byte[] array = Convert.FromBase64String(input);

#pragma warning disable SYSLIB0021 // Type or member is obsolete
#pragma warning disable CA5350 // Do Not Use Weak Cryptographic Algorithms
            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
#pragma warning restore CA5350 // Do Not Use Weak Cryptographic Algorithms
#pragma warning restore SYSLIB0021 // Type or member is obsolete

            tripleDESCryptoServiceProvider.Key = Encoding.UTF8.GetBytes(Key);
            tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
            tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;

#pragma warning disable CA5350
            ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
#pragma warning restore CA5350

            byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
            tripleDESCryptoServiceProvider.Clear();

            return Encoding.UTF8.GetString(bytes);
        }

    }
}
