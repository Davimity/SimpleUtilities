using System.Text;
using System.Security.Cryptography;

using SimpleUtilities.Security.SecureInformation;

using SAes = System.Security.Cryptography.Aes;
using SecureString = SimpleUtilities.Security.SecureInformation.Types.Texts.SecureString;

namespace SimpleUtilities.Security.Cryptography {
    ///<summary>Provides methods for AES encryption and decryption.</summary>
    ///<remarks>THREAD SAFE</remarks>
    public static class Aes{

        private static readonly ThreadLocal<SAes> AesInstance = new(SAes.Create);

        /// <summary> Encrypts a string using AES encryption </summary>
        /// <param name="input"> The string to encrypt </param>
        /// <param name="iv"> The initialization vector. Length must be 16 bytes </param>
        /// <param name="key"> The key. Length must be 32 bytes (256 bits) </param>
        /// <returns> The encrypted string </returns>
        public static string Encrypt(string input, byte[] key, byte[] iv){
            
            var aes = AesInstance.Value ?? throw new NullReferenceException("aes is null");

            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var ms = new MemoryStream()) {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    using (var sw = new StreamWriter(cs)) {
                        sw.Write(input);
                    }
                }

                return Convert.ToBase64String(ms.ToArray());
            }
            
        }

        /// <summary> Encrypts a byte array using AES encryption </summary>
        /// <param name="input"> The byte array to encrypt </param>
        /// <param name="key"> The key. Length must be 32 bytes (256 bits) </param>
        /// <param name="iv"> The initialization vector. Length must be 16 bytes </param>
        /// <returns> The encrypted byte array </returns>
        public static byte[] Encrypt(byte[] input, byte[] key, byte[] iv) {

            var aes = AesInstance.Value ?? throw new NullReferenceException("aes is null");

            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var ms = new MemoryStream()) {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    cs.Write(input, 0, input.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }

        /// <summary> Encrypts a SecureString using AES encryption </summary>
        /// <param name="input"> The SecureString to encrypt </param>
        /// <param name="key"> The key. Length must be 32 bytes (256 bits) </param>
        /// <param name="iv"> The initialization vector. Length must be 16 bytes </param>
        /// <returns> The encrypted SecureString </returns>
        public static SecureString Encrypt(SecureString input, byte[] key, byte[] iv) {

            var aes = AesInstance.Value ?? throw new NullReferenceException("aes is null");

            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var ms = new MemoryStream()) {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    var b = input.ToBytes();

                    cs.Write(b, 0, b.Length);
                    cs.FlushFinalBlock();

                    SecureData.OverwriteArray(b);
                }

                return new SecureString(input.GetEncoding().GetBytes(Convert.ToBase64String(ms.ToArray())));
            }
        }


        /// <summary> Decrypts a string using AES encryption </summary>
        /// <param name="input"> The string to decrypt </param>
        /// <param name="key"> The initialization vector. Length must be 16 bytes </param>
        /// <param name="iv"> The key. Length must be 32 bytes (256 bits) </param>
        /// <return> The decrypted string </return>
        public static string Decrypt(string input, byte[] key, byte[] iv) {

            var aes = AesInstance.Value ?? throw new NullReferenceException("aes is null");

            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor();
            using (var ms = new MemoryStream(Convert.FromBase64String(input))) {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                    using (var sr = new StreamReader(cs)) {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        /// <summary> Decrypts a byte array using AES encryption </summary>
        /// <param name="input"> The byte array to encrypt </param>
        /// <param name="key"> The initialization vector. Length must be 16 bytes </param>
        /// <param name="iv"> The key. Length must be 32 bytes (256 bits) </param>
        /// <returns> The decrypted byte array </returns>
        public static byte[] Decrypt(byte[] input, byte[] key, byte[] iv) {

            var aes = AesInstance.Value ?? throw new NullReferenceException("aes is null");

            aes.Key = key;
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream(input)) {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                    using (var msOut = new MemoryStream()) {
                        cs.CopyTo(msOut);
                        return msOut.ToArray();
                    }
                }
            }
        }

        /// <summary> Decrypts a SecureString using AES encryption </summary>
        /// <param name="input"> TheSecureString to encrypt </param>
        /// <param name="key"> The initialization vector. Length must be 16 bytes </param>
        /// <param name="iv"> The key. Length must be 32 bytes (256 bits) </param>
        /// <returns> The decrypted SecureString </returns>
        public static SecureString Decrypt(SecureString input, byte[] key, byte[] iv) {
            using (var aes = SAes.Create()) {
                aes.Key = key;
                aes.IV = iv;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(Convert.FromBase64String(input))) {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
                        using (var msOut = new MemoryStream()) {
                            cs.CopyTo(msOut);
                            return new SecureString(msOut.ToArray());
                        }
                    }
                }
            }
        }


        /// <summary> Generates a key for AES encryption </summary>
        /// <param name="input"> The input string </param>
        /// <param name="salt"> The salt string </param>
        /// <param name="iterations"> The number of iterations to generate the key </param>
        /// <returns> The generated key </returns>
        public static byte[] GenerateKey(string input, string salt, int iterations = 0) {
            var bSalt = Encoding.UTF8.GetBytes(salt);

            try {
                var rinput = Sha512.Hash(input, salt);

                if (iterations <= 0) {
                    using (var sha = SHA256.Create())
                    {
                        var hash = sha.ComputeHash(bSalt);
                        iterations = BitConverter.ToInt32(hash, 0) & 0x7FFFFFFF;
                        iterations = 10000 + (iterations % 90000);
                    }
                }

                using (var deriveBytes = new Rfc2898DeriveBytes(rinput, bSalt, iterations, HashAlgorithmName.SHA512))
                {
                    return deriveBytes.GetBytes(32);
                }
            }
            finally {
                SecureData.OverwriteArray(bSalt);
            }
        }

        /// <summary> Generates a key for AES encryption </summary>
        /// <param name="input"> The input SecureString </param>
        /// <param name="salt"> The salt SecureString </param>
        /// <param name="iterations"> The number of iterations to generate the key </param>
        /// <returns> The generated key </returns>
        public static byte[] GenerateKey(SecureString input, SecureString salt, int iterations = 0) {
            var bSalt = salt.ToBytes();

            try {
                var rinput = Sha512.Hash(input, salt);

                if (iterations <= 0) {
                    using (var sha = SHA256.Create()) {
                        var hash = sha.ComputeHash(bSalt);
                        iterations = BitConverter.ToInt32(hash, 0) & 0x7FFFFFFF;
                        iterations = 10000 + (iterations % 90000);
                    }
                }

                using (var deriveBytes = new Rfc2898DeriveBytes(rinput, bSalt, iterations, HashAlgorithmName.SHA512)) {
                    return deriveBytes.GetBytes(32);
                }
            }finally {
                SecureData.OverwriteArray(bSalt);
            }
        }

        /// <summary> Generates a key for AES encryption </summary>
        /// <param name="input"> The input byte[] </param>
        /// <param name="salt"> The salt byte[] </param>
        /// <param name="iterations"> The number of iterations to generate the key </param>
        /// <param name="destroyInputs"> True to destroy input and salt arrays after use </param>
        /// <returns> The generated key </returns>
        public static byte[] GenerateKey(byte[] input, byte[] salt, int iterations = 0, bool destroyInputs = false) {
            try {
                var rinput = Sha512.Hash(input, salt);

                if (iterations <= 0) {
                    using (var sha = SHA256.Create()) {
                        var hash = sha.ComputeHash(salt);
                        iterations = BitConverter.ToInt32(hash, 0) & 0x7FFFFFFF;
                        iterations = 10000 + (iterations % 90000);
                    }
                }

                using (var deriveBytes = new Rfc2898DeriveBytes(rinput, salt, iterations, HashAlgorithmName.SHA512)) {
                    return deriveBytes.GetBytes(32);
                }
            }
            finally {
                if(destroyInputs) {
                    SecureData.OverwriteArray(input);
                    SecureData.OverwriteArray(salt);
                }
            }
        }


        /// <summary> Generates an initialization vector for AES encryption </summary>
        /// <param name="input"> The input string </param>
        /// <param name="salt"> The salt string </param>
        /// <param name="iterations"> The number of iterations to generate the IV </param>
        /// <returns> The generated IV </returns>
        public static byte[] GenerateIv(string input, string salt, int iterations = 0) {
            var bSalt = Encoding.UTF8.GetBytes(salt);

            try {
                var rinput = Sha512.Hash(input, salt);
                
                if (iterations <= 0) {
                    using (var sha = SHA256.Create()) {
                        var hash = sha.ComputeHash(bSalt);
                        iterations = BitConverter.ToInt32(hash, 0) & 0x7FFFFFFF;
                        iterations = 10000 + (iterations % 90000);
                    }
                }

                using (var deriveBytes = new Rfc2898DeriveBytes(rinput, bSalt, iterations, HashAlgorithmName.SHA512)) {
                    return deriveBytes.GetBytes(16);
                }
            }finally {
                SecureData.OverwriteArray(bSalt);
            }
        }

        /// <summary> Generates an initialization vector for AES encryption </summary>
        /// <param name="input"> The input SecureString </param>
        /// <param name="salt"> The salt SecureString </param>
        /// <param name="iterations"> The number of iterations to generate the IV </param>
        /// <returns> The generated IV </returns>
        public static byte[] GenerateIv(SecureString input, SecureString salt, int iterations = 0) {
            var bSalt = salt.ToBytes();

            try {
                var rinput = Sha512.Hash(input, salt);

                if (iterations <= 0) {
                    using (var sha = SHA256.Create()) {
                        var hash = sha.ComputeHash(bSalt);
                        iterations = BitConverter.ToInt32(hash, 0) & 0x7FFFFFFF;
                        iterations = 10000 + (iterations % 90000);
                    }
                }

                using (var deriveBytes = new Rfc2898DeriveBytes(rinput, bSalt, iterations, HashAlgorithmName.SHA512)) {
                    return deriveBytes.GetBytes(16);
                }
            }
            finally {
                SecureData.OverwriteArray(bSalt);
            }
        }

        /// <summary> Generates an initialization vector for AES encryption </summary>
        /// <param name="input"> The input byte[] </param>
        /// <param name="salt"> The salt byte[] </param>
        /// <param name="iterations"> The number of iterations to generate the IV </param>
        /// <param name="destroyInputs"> True to destroy input and salt arrays after use </param>
        /// <returns> The generated IV </returns>
        public static byte[] GenerateIv(byte[] input, byte[] salt, int iterations = 0, bool destroyInputs = false) {
            try {
                var rinput = Sha512.Hash(input, salt);

                if (iterations <= 0) {
                    using (var sha = SHA256.Create()) {
                        var hash = sha.ComputeHash(salt);
                        iterations = BitConverter.ToInt32(hash, 0) & 0x7FFFFFFF;
                        iterations = 10000 + (iterations % 90000);
                    }
                }

                using (var deriveBytes = new Rfc2898DeriveBytes(rinput, salt, iterations, HashAlgorithmName.SHA512)) {
                    return deriveBytes.GetBytes(16);
                }
            }
            finally {
                if (destroyInputs) {
                    SecureData.OverwriteArray(input);
                    SecureData.OverwriteArray(salt);
                }
            }
        }
    }
}
