using SimpleUtilities.Security.SecureInformation;
using System.Security.Cryptography;
using System.Text;
using SimpleUtilities.Security.SecureInformation.Types.Texts;

namespace SimpleUtilities.Security.Cryptography {
    public static class Sha512{

        private static readonly SHA512 Sha = SHA512.Create();

        ///<summary> Encrypts a string using SHA512 encryption </summary>
        ///<param name="input"> The string to encrypt </param>
        ///<returns> The encrypted string </returns>
        public static string Hash(string input) {
            var data = Sha.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sb = new StringBuilder();
            foreach (var b in data) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        ///<summary> Encrypts a string using SHA512 encryption with salt </summary>
        ///<param name="input"> The string to encrypt </param>
        ///<param name="salt"> The salt to add </param>
        ///<returns> The encrypted string </returns>
        public static string Hash(string input, string salt) {
            var saltedInput = Encoding.UTF8.GetBytes(input + salt);
            var data = Sha.ComputeHash(saltedInput);

            var sb = new StringBuilder();
            foreach (var b in data) sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        ///<summary> Encrypts a byte array using SHA512 encryption </summary>
        ///<param name="input"> The byte array to encrypt </param>
        ///<param name="destroyArray"> If true, the input array will be destroyed after the operation </param>
        ///<returns> The encrypted byte array </returns>
        public static byte[] Hash(byte[] input, bool destroyArray = false){
            var data = Sha.ComputeHash(input);

            if (destroyArray) SecureData.OverwriteArray(input);
            return data;
        }

        ///<summary> Encrypts a byte array using SHA512 encryption with salt </summary>
        ///<param name="input"> The byte array to encrypt </param>
        ///<param name="salt"> The salt to add </param>
        ///<param name="destroyArrays"> If true, the input and salt arrays will be destroyed after the operation </param>
        ///<returns> The encrypted byte array </returns>
        public static byte[] Hash(byte[] input, byte[] salt, bool destroyArrays = false){
            using (var sha512 = SHA512.Create()) {
                var combined = new byte[input.Length + salt.Length];

                Buffer.BlockCopy(input, 0, combined, 0, input.Length);
                Buffer.BlockCopy(salt, 0, combined, input.Length, salt.Length);

                var data = sha512.ComputeHash(combined);

                SecureData.OverwriteArray(combined);

                if(!destroyArrays) return data;

                SecureData.OverwriteArray(input);
                SecureData.OverwriteArray(salt);
                
                return data;
            }
        }

        ///<summary> Encrypts a SecureString using SHA512 encryption </summary>
        ///<param name="input"> The SecureString to encrypt </param>
        ///<param name="destroyInput"> If true, the input SecureString will be destroyed after the operation </param>
        ///<returns> The encrypted SecureString </returns>
        public static SecureString Hash(SecureString input, bool destroyInput = false) {
            var data = Sha.ComputeHash(input.ToBytes());

            var sb = new StringBuilder();
            foreach (var b in data) sb.Append(b.ToString("x2"));

            if(destroyInput) input.Dispose();

            return new SecureString(sb.ToString());
        }

        ///<summary> Encrypts a SecureString using SHA512 encryption with salt </summary>
        ///<param name="input"> The SecureString to encrypt </param>
        ///<param name="salt"> The salt to add </param>
        ///<param name="destroyInput"> If true, the input SecureString will be destroyed after the operation </param>
        ///<returns> The encrypted SecureString </returns>
        public static SecureString Hash(SecureString input, SecureString salt, bool destroyInput = false) {
            var saltedInput = (input + salt).ToBytes();
            var data = Sha.ComputeHash(saltedInput);

            var sb = new StringBuilder();
            foreach (var b in data) sb.Append(b.ToString("x2"));

            if (destroyInput) input.Dispose();

            return new SecureString(sb.ToString());
        }
    }
}
