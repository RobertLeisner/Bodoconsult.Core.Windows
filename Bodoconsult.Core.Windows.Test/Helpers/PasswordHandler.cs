using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bodoconsult.Core.Windows.Test.Helpers
{
    /// <summary>
    /// Encrypts und decrypts passwords for console application start parameters
    /// </summary>
    public class PasswordHandler
    {

        /// <summary>
        /// Keys used for symmetric data encryption 
        /// </summary>
        private static readonly byte[] KeyBytes = Encoding.ASCII.GetBytes("wino1967");
        private static readonly byte[] KeyBytes2 = Encoding.ASCII.GetBytes("1967wino");
        private static readonly byte[] KeyBytes3 = Encoding.ASCII.GetBytes("19wino67");

        /// <summary>
        /// Encrypt as string with 
        /// </summary>
        /// <param name="originalString">Original string</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string originalString)
        {
            if (string.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException
                    // ReSharper disable NotResolvedInText
                    ("The string which needs to be encrypted can not be null.");
                // ReSharper restore NotResolvedInText
            }
            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(KeyBytes, KeyBytes), CryptoStreamMode.Write);
            var writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }



        /// <summary>
        /// Encrypt as string with 
        /// </summary>
        /// <param name="originalString">Original string</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt2(string originalString)
        {
            if (string.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException
                    // ReSharper disable NotResolvedInText
                    ("The string which needs to be encrypted can not be null.");
                // ReSharper restore NotResolvedInText
            }
            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(KeyBytes2, KeyBytes2), CryptoStreamMode.Write);
            var writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }


        /// <summary>
        /// Encrypt as string with 
        /// </summary>
        /// <param name="originalString">Original string</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt3(string originalString)
        {
            if (string.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException
                    // ReSharper disable NotResolvedInText
                    ("The string which needs to be encrypted can not be null.");
                // ReSharper restore NotResolvedInText
            }
            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(KeyBytes3, KeyBytes3), CryptoStreamMode.Write);
            var writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }



        /// <summary>
        /// Decrypt as string
        /// </summary>
        /// <param name="cryptedString">Crypted string</param>
        /// <returns>Original string</returns>
        public static string Decrypt(string cryptedString)
        {
            if (string.IsNullOrEmpty(cryptedString))
            {
                return null;
            }
            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream
                (Convert.FromBase64String(cryptedString));
            var cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(KeyBytes, KeyBytes), CryptoStreamMode.Read);
            var reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }



        /// <summary>
        /// Decrypt as string
        /// </summary>
        /// <param name="cryptedString">Crypted string</param>
        /// <returns>Original string</returns>
        public static string Decrypt2(string cryptedString)
        {
            if (string.IsNullOrEmpty(cryptedString))
            {
                return null;
            }
            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream
                (Convert.FromBase64String(cryptedString));
            var cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(KeyBytes2, KeyBytes2), CryptoStreamMode.Read);
            var reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }


        /// <summary>
        /// Decrypt as string
        /// </summary>
        /// <param name="cryptedString">Crypted string</param>
        /// <returns>Original string</returns>
        public static string Decrypt3(string cryptedString)
        {
            if (string.IsNullOrEmpty(cryptedString))
            {
                return null;
            }
            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream
                (Convert.FromBase64String(cryptedString));
            var cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(KeyBytes3, KeyBytes3), CryptoStreamMode.Read);
            var reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Compare two hash values a and b: if a equals b return true else false
        /// </summary>
        /// <param name="a">hash value a</param>
        /// <param name="b">hash value b</param>
        /// <returns></returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (var i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt) { IterationCount = iterations };
            return pbkdf2.GetBytes(outputBytes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="salt"></param>
        /// <param name="hashBytes"></param>
        /// <param name="pbkdf2Iterations"></param>
        /// <returns>Base64 encoded string</returns>
        public static string CreateHash(string value, string salt, int hashBytes, int pbkdf2Iterations)
        {

            var saltBytes = Convert.FromBase64String(salt);

            // Hash the value and encode the parameters
            var hash = Pbkdf2(value, saltBytes, pbkdf2Iterations, hashBytes);

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Create a salt for hashing
        /// </summary>
        /// <param name="saltBytes"></param>
        /// <returns></returns>
        public static string CreateSalt(int saltBytes)
        {
            // Generate a random salt
            var csprng = new RNGCryptoServiceProvider();
            var salt = new byte[saltBytes];
            csprng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }



        /// <summary>
        /// Hashes a value and compares with another hashed value
        /// </summary>
        /// <param name="pureVal"></param>
        /// <param name="saltVal">Salt</param>
        /// <param name="hashVal">Hashed value to compare with</param>
        /// <param name="pbkdf2Iterations"></param>
        /// <returns></returns>
        public static bool ValidateHash(string pureVal, string saltVal, string hashVal, int pbkdf2Iterations)
        {
            try
            {
                var salt = Convert.FromBase64String(saltVal);
                var hash = Convert.FromBase64String(hashVal);

                var testHash = Pbkdf2(pureVal, salt, pbkdf2Iterations, hash.Length);
                return SlowEquals(hash, testHash);
            }
            catch
            {
                return false;
            }
        }



    }
}