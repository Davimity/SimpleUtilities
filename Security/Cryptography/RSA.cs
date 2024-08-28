using System;
using System.Security.Cryptography;
using System.Text;

namespace SimpleUtilities.Security.Cryptography{

    public static class RSA{


        ///<summary> Encrypts a string using RSA encryption </summary>
        ///<param name="input"> The string to encrypt </param>
        ///<param name="rsaParams"> The RSA parameters, containing the public key. Example of RSAParameters: new RSAParameters { Exponent = new byte[] { 1, 0, 1 }, Modulus = new byte[] { 1, 0, 1 } } </param>
        ///<returns> The encrypted string </returns>
        public static string RSAEncrypt(string input, RSAParameters rsaParams)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                byte[] data = Encoding.UTF8.GetBytes(input);
                byte[] encryptedData = rsa.Encrypt(data, false);
                return Convert.ToBase64String(encryptedData);
            }
        }

        ///<summary> Decrypts a string using RSA encryption </summary>
        ///<param name="input"> The string to decrypt </param>
        ///<param name="rsaParams"> The RSA parameters, containing the private key. Example of RSAParameters: new RSAParameters { Exponent = new byte[] { 1, 0, 1 }, Modulus = new byte[] { 1, 0, 1 } } </param>
        ///<returns> The decrypted string </returns>
        public static string RSADecrypt(string input, RSAParameters rsaParams)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                byte[] data = Convert.FromBase64String(input);
                byte[] decryptedData = rsa.Decrypt(data, false);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

    }
}
