using SimpleUtilities.Security.SecureInformation;
using SimpleUtilities.Security.SecureInformation.Types;
using System.Security.Cryptography;
using System.Text;

using SecureString = SimpleUtilities.Security.SecureInformation.Types.SecureString;

namespace SimpleUtilities.Security.Cryptography
{
    public static class SHA{

        #region SHA256

            ///<summary> Encrypts a string using SHA256 encryption </summary>
            ///<param name="input"> The string to encrypt </param>
            ///<returns> The encrypted string </returns>
            public static string SHA256Hash(string input){
                using (SHA256 sha256 = SHA256.Create()){
                    byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < data.Length; i++)
                        sb.Append(data[i].ToString("x2"));

                    return sb.ToString();
                }
            }

            ///<summary> Encrypts a byte array using SHA256 encryption </summary>
            ///<param name="input"> The byte array to encrypt </param>
            ///<param name="destroyArray"> If true, the input array will be destroyed after the operation </param>
            ///<returns> The encrypted byte array </returns>
            public static byte[] SHA256Hash(byte[] input, bool destroyArray = false){
                using (SHA256 sha256 = SHA256.Create()){
                    byte[] data = sha256.ComputeHash(input);
                    if (destroyArray) SecureData.OverwriteArray(input);
                    return data;
                }
            }

            ///<summary> Encrypts a SecureData using SHA256 encryption </summary>
            ///<param name="input"> The SecureData to encrypt </param>
            ///<returns> The encrypted SecureString </returns>
            public static SecureString SHA256Hash(SecureData input){
                using (SHA256 sha256 = SHA256.Create()){
                    return new SecureString(sha256.ComputeHash(input.ToBytes()));
                }
            }

            ///<summary> Encrypts a SecureArray using SHA256 encryption </summary>
            ///<param name="input"> The SecureArray to encrypt </param>
            ///<returns> The encrypted SecureArray </returns>
            public static SecureArray<byte> SHA256Hash(SecureArray<byte> input){
                using (SHA256 sha256 = SHA256.Create()){
                    return new SecureArray<byte>(sha256.ComputeHash(input.Data));
                }
            }

            ///<summary> Encrypts a string using SHA256 encryption with salt </summary>
            ///<param name="input"> The string to encrypt </param>
            ///<param name="salt"> The salt to add </param>
            ///<returns> The encrypted string </returns>
            public static string SHA256HashSalt(string input, string salt){
                using (SHA256 sha256 = SHA256.Create()){
                    byte[] saltedInput = Encoding.UTF8.GetBytes(input + salt);
                    byte[] data = sha256.ComputeHash(saltedInput);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < data.Length; i++){
                        sb.Append(data[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }

            ///<summary> Encrypts a SecureString using SHA256 encryption </summary>
            ///<param name="input"> The SecureString to encrypt </param>
            ///<param name="salt"> The salt to add </param>
            ///<param name="destroyArrays"> If true, the input and salt arrays will be destroyed after the operation </param>
            ///<returns> The encrypted SecureString </returns>
            public static byte[] SHA256HashSalt(byte[] input, byte[] salt, bool destroyArrays = false){
                using (SHA256 sha256 = SHA256.Create()){
                    byte[] combined = new byte[input.Length + salt.Length];

                    Buffer.BlockCopy(input, 0, combined, 0, input.Length);
                    Buffer.BlockCopy(salt, 0, combined, input.Length, salt.Length);

                    byte[] data = sha256.ComputeHash(combined);

                    SecureData.OverwriteArray(combined);
                    if (destroyArrays){
                        SecureData.OverwriteArray(input);
                        SecureData.OverwriteArray(salt);
                    }

                    return data;
                }
            }

            ///<summary> Encrypts a SecureData using SHA256 encryption with salt </summary>
            ///<param name="input"> The SecureData to encrypt </param>
            ///<param name="salt"> The salt to add </param>
            ///<returns> The encrypted SecureData </returns>
            public static SecureString SHA256HashSalt(SecureData input, SecureData salt){
                using (SHA256 sha256 = SHA256.Create()){
                    byte[] saltedInput = new byte[input.Length + salt.Length];

                    Buffer.BlockCopy(input.ToBytes(), 0, saltedInput, 0, input.Length);
                    Buffer.BlockCopy(salt.ToBytes(), 0, saltedInput, input.Length, salt.Length);

                    return new SecureString(sha256.ComputeHash(saltedInput));
                }
            }

            ///<summary> Encrypts a SecureArray using SHA256 encryption with salt </summary>
            ///<param name="input"> The SecureArray to encrypt </param>
            ///<param name="salt"> The salt to add </param>
            ///<returns> The encrypted SecureArray </returns>
            public static SecureArray<byte> SHA256HashSalt(SecureArray<byte> input, SecureArray<byte> salt){
                using (SHA256 sha256 = SHA256.Create()){
                    byte[] combined = new byte[input.Length + salt.Length];

                    Buffer.BlockCopy(input.Data, 0, combined, 0, input.Length);
                    Buffer.BlockCopy(salt.Data, 0, combined, input.Length, salt.Length);

                    return new SecureArray<byte>(sha256.ComputeHash(combined));
                }
            }

        #endregion

        #region SHA512

            ///<summary> Encrypts a string using SHA512 encryption </summary>
            ///<param name="input"> The string to encrypt </param>
            ///<returns> The encrypted string </returns>
            public static string SHA512Hash(string input)
            {
                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] data = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sb.Append(data[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }

            ///<summary> Encrypts a byte array using SHA512 encryption </summary>
            ///<param name="input"> The byte array to encrypt </param>
            ///<param name="destroyArray"> If true, the input array will be destroyed after the operation </param>
            ///<returns> The encrypted byte array </returns>
            public static byte[] SHA512Hash(byte[] input, bool destroyArray = false){
                using (SHA512 sha512 = SHA512.Create()){
                    byte[] data = sha512.ComputeHash(input);
                    if (destroyArray) SecureData.OverwriteArray(input);
                    return data;
                }
            }

            ///<summary> Encrypts a SecureData using SHA512 encryption </summary>
            ///<param name="input"> The SecureData to encrypt </param>
            ///<returns> The encrypted SecureData </returns>
            public static SecureString SHA512Hash(SecureData input){
                using (SHA512 sha512 = SHA512.Create()){
                    return new SecureString(sha512.ComputeHash(input.ToBytes()));
                }
            }

            ///<summary> Encrypts a SecureArray using SHA512 encryption </summary>
            ///<param name="input"> The SecureArray to encrypt </param>
            ///<returns> The encrypted SecureArray </returns>
            public static SecureArray<byte> SHA512Hash(SecureArray<byte> input){
                using (SHA512 sha512 = SHA512.Create()){
                    return new SecureArray<byte>(sha512.ComputeHash(input.Data));
                }
            }

            ///<summary> Encrypts a string using SHA512 encryption with salt </summary>
            ///<param name="input"> The string to encrypt </param>
            ///<param name="salt"> The salt to add </param>
            ///<returns> The encrypted string </returns>
            public static string SHA512HashSalt(string input, string salt){
                using (SHA512 sha512 = SHA512.Create()){
                    byte[] saltedInput = Encoding.UTF8.GetBytes(input + salt);
                    byte[] data = sha512.ComputeHash(saltedInput);

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                        sb.Append(data[i].ToString("x2"));
                    
                    return sb.ToString();
                }
            }

            ///<summary> Encrypts a byte array using SHA512 encryption with salt </summary>
            ///<param name="input"> The byte array to encrypt </param>
            ///<param name="salt"> The salt to add </param>
            ///<param name="destroyArrays"> If true, the input and salt arrays will be destroyed after the operation </param>
            ///<returns> The encrypted byte array </returns>
            public static byte[] SHA512HashSalt(byte[] input, byte[] salt, bool destroyArrays = false){
                using (SHA512 sha512 = SHA512.Create()){

                    byte[] combined = new byte[input.Length + salt.Length];

                    Buffer.BlockCopy(input, 0, combined, 0, input.Length);
                    Buffer.BlockCopy(salt, 0, combined, input.Length, salt.Length);

                    byte[] data = sha512.ComputeHash(combined);

                    SecureData.OverwriteArray(combined);
                    if (destroyArrays){
                        SecureData.OverwriteArray(input);
                        SecureData.OverwriteArray(salt);
                    }

                    return data;
                }
            }

            ///<summary> Encrypts a SecureData using SHA512 encryption with salt </summary>
            ///<param name="input"> The SecureData to encrypt </param>
            ///<param name="salt"> The salt to add </param>
            ///<returns> The encrypted SecureData </returns>
            public static SecureString SHA512HashSalt(SecureData input, SecureData salt){
                using (SHA512 sha512 = SHA512.Create()){
                    byte[] saltedInput = new byte[input.Length + salt.Length];

                    Buffer.BlockCopy(input.ToBytes(), 0, saltedInput, 0, input.Length);
                    Buffer.BlockCopy(salt.ToBytes(), 0, saltedInput, input.Length, salt.Length);

                    return new SecureString(sha512.ComputeHash(saltedInput));
                }
            }
        
            ///<summary> Encrypts a SecureArray using SHA512 encryption with salt </summary>
            ///<param name="input"> The SecureArray to encrypt </param>
            ///<param name="salt"> The salt to add </param>
            ///<returns> The encrypted SecureArray </returns>
            public static SecureArray<byte> SHA512HashSalt(SecureArray<byte> input, SecureArray<byte> salt){
                using (SHA512 sha512 = SHA512.Create()){
                    byte[] combined = new byte[input.Length + salt.Length];

                    Buffer.BlockCopy(input.Data, 0, combined, 0, input.Length);
                    Buffer.BlockCopy(salt.Data, 0, combined, input.Length, salt.Length);

                    return new SecureArray<byte>(sha512.ComputeHash(combined));
                }
            }

        #endregion
    }
}
