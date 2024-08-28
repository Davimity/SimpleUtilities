using SimpleUtilities.Exceptions.SecurityExceptions.CryptographycExceptions;
using SimpleUtilities.Security.SecureInformation.Types;
using System.Security.Cryptography;
using System.Text;

using SecureString = SimpleUtilities.Security.SecureInformation.Types.SecureString;

namespace SimpleUtilities.Security.Cryptography
{
    public static class AES{

        /// <summary> Encrypts a string using AES encryption </summary>
        /// <param name="input"> The string to encrypt </param>
        /// <param name="iv"> The initialization vector. Length must be 16 bytes </param>
        /// <param name="key"> The key. Length must be 32 bytes (256 bits) </param>
        /// <returns> The encrypted string </returns>
        public static string AESEncrypt(string input, byte[] key, byte[] iv){

            if (key == null || key.Length != 32)
                throw new ArgumentException("Key length must be 32 bytes (256 bits).");
            if (iv == null || iv.Length != 16)
                throw new ArgumentException("IV length must be 16 bytes.");

            using (Aes aes = Aes.Create()){
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream()){
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)){
                        using (StreamWriter sw = new StreamWriter(cs)){
                            sw.Write(input);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary> Encrypts a byte array using AES encryption </summary>
        /// <param name="input"> The byte array to encrypt </param>
        /// <param name="key"> The key. Length must be 32 bytes (256 bits) </param>
        /// <param name="iv"> The initialization vector. Length must be 16 bytes </param>
        /// <returns> The encrypted byte array </returns>
        public static byte[] AESEncrypt(byte[] input, byte[] key, byte[] iv){

            if (key == null || key.Length != 32)
                throw new ArgumentException("Key length must be 32 bytes (256 bits).");
            if (iv == null || iv.Length != 16)
                throw new ArgumentException("IV length must be 16 bytes.");

            using (Aes aes = Aes.Create()){
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream()){
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)){
                        cs.Write(input, 0, input.Length);
                        cs.FlushFinalBlock();
                    }
                    return ms.ToArray();
                }
            }
        }

        /// <summary> Encrypts a SecureString using AES encryption </summary>
        /// <param name="input"> The SecureString to encrypt </param>
        /// <param name="key"> The key. Length must be 32 bytes (256 bits) </param>
        /// <param name="iv"> The initialization vector. Length must be 16 bytes </param>
        /// <returns> The encrypted SecureString </returns>
        public static SecureString AESEncrypt(SecureString input, byte[] key, byte[] iv){
            if (key == null || key.Length != 32)
                throw new ArgumentException("Key length must be 32 bytes (256 bits).");
            if (iv == null || iv.Length != 16)
                throw new ArgumentException("IV length must be 16 bytes.");

            using (Aes aes = Aes.Create()){
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream()){
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)){
                        byte[] bytes = input.ToBytes();

                        cs.Write(bytes, 0, bytes.Length);
                        cs.FlushFinalBlock();

                        SecureData.OverwriteArray(bytes);
                    }

                    return new SecureString(ms.ToArray());
                }
            }
        }

        /// <summary> Decrypts a string using AES encryption </summary>
        /// <param> The string to decrypt </param>
        /// <param> The initialization vector. Length must be 16 bytes </param>
        /// <param> The key. Length must be 32 bytes (256 bits) </param>
        /// <return> The decrypted string </return>
        public static string AESDecrypt(string input, byte[] key, byte[] iv){

            if (key == null || key.Length != 32)
                throw new ArgumentException("Key length must be 32 bytes (256 bits).");
            if (iv == null || iv.Length != 16)
                throw new ArgumentException("IV length must be 16 bytes.");

            using (Aes aes = Aes.Create()){

                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(input))){
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)){
                        using (StreamReader sr = new StreamReader(cs)){
                            try{
                                return sr.ReadToEnd();
                            }
                            catch (Exception e){
                                throw new AESDecryptException("Error decrypting the SecureString. Error message: " + e.Message);
                            }         
                        }
                    }
                }
            }
        }

        /// <summary> Decrypts a byte array using AES encryption </summary>
        /// <param name="input"> The byte array to encrypt </param>
        /// <param> The initialization vector. Length must be 16 bytes </param>
        /// <param> The key. Length must be 32 bytes (256 bits) </param>
        /// <returns> The decrypted byte array </returns>
        public static byte[] AESDecrypt(byte[] input, byte[] key, byte[] iv){

            if (key == null || key.Length != 32)
                throw new ArgumentException("Key length must be 32 bytes (256 bits).");
            if (iv == null || iv.Length != 16)
                throw new ArgumentException("IV length must be 16 bytes.");

            using (Aes aes = Aes.Create()){

                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(input)){
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)){
                        using (MemoryStream msOut = new MemoryStream()){
                            cs.CopyTo(msOut);
                            return msOut.ToArray();
                        }
                    }
                }
            }

        }

        /// <summary> Decrypts a SecureString using AES encryption </summary>
        /// <param name="input"> TheSecureString to encrypt </param>
        /// <param> The initialization vector. Length must be 16 bytes </param>
        /// <param> The key. Length must be 32 bytes (256 bits) </param>
        /// <returns> The decrypted SecureString </returns>
        public static SecureString AESDecrypt(SecureString input, byte[] key, byte[] iv){
            if (key == null || key.Length != 32)
                throw new ArgumentException("Key length must be 32 bytes (256 bits).");
            if (iv == null || iv.Length != 16)
                throw new ArgumentException("IV length must be 16 bytes.");

            using (Aes aes = Aes.Create())
            {

                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] bytes = input.ToBytes();

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream msOut = new MemoryStream())
                        {
                            cs.CopyTo(msOut);
                            SecureData.OverwriteArray(bytes);
                            return new SecureString(msOut.ToArray());
                        }
                    }
                }
            }

        }

        /// <summary> Generates a key for AES encryption </summary>
        /// <param name="input"> The input string </param>
        /// <param name="salt"> The salt string </param>
        /// <returns> The generated key </returns>
        public static byte[] GenerateKey(string input, string salt){

            string combinedInput = input + salt;
            byte[] bSalt = Encoding.UTF8.GetBytes(salt);

            int iterations;

            using(SHA256 sha = SHA256.Create()){
                byte[] hash = sha.ComputeHash(bSalt);
                iterations = BitConverter.ToInt32(hash, 0) & 0x7FFFFFFF;
                iterations = 10000 + (iterations % 90000);
            }

            byte[] key;

            using (var deriveBytes = new Rfc2898DeriveBytes(combinedInput, bSalt, iterations, HashAlgorithmName.SHA512)){
                key = deriveBytes.GetBytes(32); // AES-256 key size
            }

            return key;
        }

        /// <summary> Generates an initialization vector for AES encryption </summary>
        /// <param name="input"> The input string </param>
        /// <param name="salt"> The salt string </param>
        /// <returns> The generated IV </returns>
        public static byte[] GenerateIV(string input, string salt){

            string combinedInput = input + salt;
            byte[] bSalt = Encoding.UTF8.GetBytes(salt);

            int iterations;

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(bSalt);
                iterations = BitConverter.ToInt32(hash, 0) & 0x7FFFFFFF;
                iterations = 10000 + (iterations % 90000);
            }

            byte[] iv;

            using (var deriveBytes = new Rfc2898DeriveBytes(combinedInput, bSalt, iterations, HashAlgorithmName.SHA512)){
                iv = deriveBytes.GetBytes(16); // AES-128 IV size
            }

            return iv;
        }

        /// <summary> Generates a key for AES encryption </summary>
        /// <param name="input"> The input SecureString </param>
        /// <param name="salt"> The salt SecureString </param>
        /// <returns> The generated key </returns>
        public static byte[] GenerateKey(SecureString input, SecureString salt){

            SecureString combinedInput = new SecureString();

            combinedInput.Append(input);
            combinedInput.Append(salt);

            byte[] bSalt = salt.ToBytes();

            int iterations;

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(bSalt);
                iterations = BitConverter.ToInt32(hash, 0) & 0x7FFFFFFF;
                iterations = 10000 + (iterations % 90000);
            }

            byte[] key;

            string str = combinedInput.ToString();

            using (var deriveBytes = new Rfc2898DeriveBytes(str, bSalt, iterations, HashAlgorithmName.SHA512)){
                key = deriveBytes.GetBytes(32); // AES-256 key size
            }

            SecureData.OverwriteString(str);
            SecureData.OverwriteArray(bSalt);

            combinedInput.Dispose();

            return key;
        }

        /// <summary> Generates an initialization vector for AES encryption </summary>
        /// <param name="input"> The input SecureString </param>
        /// <param name="salt"> The salt SecureString </param>
        /// <returns> The generated IV </returns>
        public static byte[] GenerateIV(SecureString input, SecureString salt){

            SecureString combinedInput = new SecureString();

            combinedInput.Append(input);
            combinedInput.Append(salt);

            byte[] bSalt = salt.ToBytes();

            int iterations;

            using (SHA256 sha = SHA256.Create()){
                byte[] hash = sha.ComputeHash(bSalt);
                iterations = BitConverter.ToInt32(hash, 0) & 0x7FFFFFFF;
                iterations = 10000 + (iterations % 90000);
            }

            byte[] iv;

            string str = combinedInput.ToString();

            using (var deriveBytes = new Rfc2898DeriveBytes(str, bSalt, iterations, HashAlgorithmName.SHA512)){
                iv = deriveBytes.GetBytes(16); // AES-128 IV size
            }

            SecureData.OverwriteString(str);
            SecureData.OverwriteArray(bSalt);

            combinedInput.Dispose();

            return iv;
        }

    }
}
