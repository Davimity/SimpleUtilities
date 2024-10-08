using SimpleUtilities.Security.Cryptography;
using SimpleUtilities.Security.SecureInformation.Types.Numerics;
using System.Text;
using SimpleUtilities.Security.SecureInformation;
using SecureString = SimpleUtilities.Security.SecureInformation.Types.Texts.SecureString;

namespace SimpleUtilities.Utilities.Files
{
    public static class File{

        ///<summary> Reads a file </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<param name="linesToRead"> The lines to read </param>
        ///<param name="decrypt"> If true, the content will be decrypted </param>
        ///<param name="includeLineBreaks"> If true, the line breaks will be included in the output </param>">
        /// <param name="key"> The key to use for decryption </param>
        /// <param name="iv"> The IV to use for decryption </param>
        /// <param name="salt"> The salt to use for decryption </param>
        ///<returns> The file content as an array of lines </returns>
        public static string[] Read(string filePath, int[]? linesToRead = null, bool decrypt = false, bool includeLineBreaks = false, byte[]? key = null, byte[]? iv = null, byte[]? salt = null) {
            if (!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File not found");

            string allText;

            if (decrypt) {
                key ??= Aes.GenerateKey(filePath, filePath);
                iv ??= Aes.GenerateIv(filePath, filePath);
                if (salt == null || salt.Length != 16) salt = Aes.GenerateIv(key, iv);

                var encryptedBytes = System.IO.File.ReadAllBytes(filePath);
                var decryptedBytes = Aes.Decrypt(encryptedBytes, Aes.GenerateKey(iv, salt), Aes.GenerateIv(salt, key));

                allText = Encoding.UTF8.GetString(decryptedBytes);

                SecureData.OverwriteArray(decryptedBytes, encryptedBytes);
            }
            else 
                allText = System.IO.File.ReadAllText(filePath);

            var lines = allText.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
            if(includeLineBreaks) for (var i = 0; i < lines.Length - 1; i++) lines[i] = lines[i] + '\n';

            string[] selectedLines;

            if(linesToRead != null) {
                selectedLines = new string[linesToRead.Length];
                for (var i = 0; i < linesToRead.Length; i++) selectedLines[i] = lines[linesToRead[i]];
            }
            else selectedLines = lines;

            return selectedLines;
        }

        ///<summary> Reads a file </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<param name="decrypt"> If true, the content will be decrypted </param>
        /// <param name="linesToRead"> The lines to read </param>
        /// <param name="includeLineBreaks"> If true, the line breaks will be included in the output </param>
        /// <param name="key"> The key to use for decryption </param>
        /// <param name="iv"> The IV to use for decryption </param>
        /// <param name="salt"> The salt to use for decryption </param>
        /// <returns> The file content as an array of lines </returns>
        public static SecureString[] Read(SecureString filePath, SecureInt[]? linesToRead = null, bool decrypt = false, bool includeLineBreaks = false, byte[]? key = null, byte[]? iv = null, byte[]? salt = null) {
            if (!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File not found");

            SecureString allText;

            if (decrypt) {
                key ??= Aes.GenerateKey(filePath, filePath);
                iv ??= Aes.GenerateIv(filePath, filePath);
                if (salt == null || salt.Length != 16) salt = Aes.GenerateIv(key, iv);

                var encryptedBytes = System.IO.File.ReadAllBytes(filePath);
                var decryptedBytes = Aes.Decrypt(encryptedBytes, Aes.GenerateKey(iv, salt), Aes.GenerateIv(salt, key));

                allText = new SecureString(decryptedBytes);

                SecureData.OverwriteArray(decryptedBytes, encryptedBytes);
            }
            else
                allText = new SecureString(System.IO.File.ReadAllBytes(filePath));

            var lines = allText.Split("\r\n", "\r", "\n");
            if (includeLineBreaks) for (var i = 0; i < lines.Length - 1; i++) lines[i] += '\n';

            SecureString[] selectedLines;

            if (linesToRead != null) {
                selectedLines = new SecureString[linesToRead.Length];
                for (var i = 0; i < linesToRead.Length; i++) selectedLines[i] = lines[linesToRead[i]];
            }
            else selectedLines = lines;

            return selectedLines;
        }


        ///<summary> Writes multiple strings to a file </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<param name="content"> The content to write </param>
        ///<param name="append"> If true, the content will be appended to the file, otherwise it will be overwritten </param>
        ///<param name="createIfNotExist"> If true, the file will be created if it does not exist </param>
        ///<param name="encrypt"> If true, the content will be encrypted </param>
        ///<param name="key"> The key to use for encryption </param>
        ///<param name="iv"> The IV to use for encryption </param>
        ///<param name="salt"> The salt to use for encryption </param>
        ///<remarks>The method does not write each string of content in a new line, if you want to write in a new line, add "\n" at the end of each string</remarks>
        public static void Write(string filePath, string[] content, bool append = true, bool createIfNotExist = true, bool encrypt = false, byte[]? key = null, byte[]? iv = null, byte[]? salt = null){

            if (!System.IO.File.Exists(filePath)) {
                if (createIfNotExist) Create(filePath);
                else throw new FileNotFoundException("File not found");
            }

            var linesToWrite = content.SelectMany(s => s.Split(["\r\n", "\r", "\n"], StringSplitOptions.None)).ToList();

            var lines = linesToWrite.ToArray();
            linesToWrite.Clear();

            if(lines.Length == 0) return;

            var textToWrite = string.Join("\n", content);

            if (encrypt){

                key ??= Aes.GenerateKey(filePath, filePath);
                iv ??= Aes.GenerateIv(filePath, filePath);
                if (salt == null || salt.Length != 16) salt = Aes.GenerateIv(key, iv);

                var existingText = "";

                if (append) {
                    var existingBytes = System.IO.File.ReadAllBytes(filePath);
                    var existingDecryptedBytes = Aes.Decrypt(existingBytes, Aes.GenerateKey(iv, salt), Aes.GenerateIv(salt, key));
                    existingText = Encoding.UTF8.GetString(existingDecryptedBytes);

                    SecureData.OverwriteArray(existingDecryptedBytes, existingBytes);
                }

                var combinedText = existingText + textToWrite;
                var combinedBytes = Encoding.UTF8.GetBytes(combinedText);
                var encryptedBytes = Aes.Encrypt(combinedBytes, Aes.GenerateKey(iv, salt), Aes.GenerateIv(salt, key));

                System.IO.File.WriteAllBytes(filePath, encryptedBytes);

                SecureData.OverwriteArray(combinedBytes, encryptedBytes);

                return;
            }

            if(append) System.IO.File.AppendAllText(filePath, textToWrite);
            else System.IO.File.WriteAllText(filePath, textToWrite);
        }

        ///<summary> Writes multiple strings to a file </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<param name="content"> The content to write </param>
        ///<param name="append"> If true, the content will be appended to the file, otherwise it will be overwritten </param>
        ///<param name="createIfNotExist"> If true, the file will be created if it does not exist </param>
        ///<param name="encrypt"> If true, the content will be encrypted </param>
        ///<param name="key"> The key to use for encryption </param>
        ///<param name="iv"> The IV to use for encryption </param>
        ///<param name="salt"> The salt to use for encryption </param>
        ///<remarks>The method does not write each string of content in a new line, if you want to write in a new line, add "\n" at the end of each string</remarks>
        public static void Write(SecureString filePath, SecureString[] content, bool append = true, bool createIfNotExist = true, bool encrypt = false, byte[]? key = null, byte[]? iv = null, byte[]? salt = null){

            if (!System.IO.File.Exists(filePath)) {
                if (createIfNotExist) Create(filePath);
                else throw new FileNotFoundException("File not found");
            }

            var linesToWrite = content.SelectMany(ss => ss.Split("\r\n", "\n", "\r")).ToList();

            var lines = linesToWrite.ToArray();
            linesToWrite.Clear();

            if (lines.Length == 0) return;

            var textToWrite = SecureString.Join(content, (SecureString)"\n");

            if (encrypt) {

                key ??= Aes.GenerateKey(filePath, filePath);
                iv ??= Aes.GenerateIv(filePath, filePath);
                if (salt == null || salt.Length != 16) salt = Aes.GenerateIv(key, iv);

                var existingText = new SecureString();

                if (append) {
                    var existingBytes = System.IO.File.ReadAllBytes(filePath);
                    var existingDecryptedBytes = Aes.Decrypt(existingBytes, Aes.GenerateKey(iv, salt), Aes.GenerateIv(salt, key));
                    existingText = new SecureString(existingDecryptedBytes);

                    SecureData.OverwriteArray(existingDecryptedBytes, existingBytes);
                }

                var combinedText = existingText + textToWrite;
                var combinedBytes = combinedText.ToBytes();
                var encryptedBytes = Aes.Encrypt(combinedBytes, Aes.GenerateKey(iv, salt), Aes.GenerateIv(salt, key));

                System.IO.File.WriteAllBytes(filePath, encryptedBytes);

                SecureData.OverwriteArray(combinedBytes, encryptedBytes);

                return;
            }

            if (append) System.IO.File.AppendAllText(filePath, textToWrite);
            else System.IO.File.WriteAllText(filePath, textToWrite);
        }


        ///<summary> Renames a file </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<param name="newName"> The new name of the file (not the new path, only the new name) </param>
        ///<returns> The new path to the file </returns>
        public static string Rename(string filePath, string newName){
            if(!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File not found");

            var parentDirectory = Directory.GetParent(filePath);
            string parentPath;

            if(parentDirectory != null) parentPath = parentDirectory.FullName; 
            else throw new IOException("Invalid path");

            var newPath = Path.Combine(parentPath, newName);
            if(System.IO.File.Exists(newPath)) throw new IOException("File already exists");
            System.IO.File.Move(filePath, newPath);

            return newPath;
        }

        ///<summary> Renames a file </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<param name="newName"> The new name of the file (not the new path, only the new name) </param>
        ///<returns> The new path to the file </returns>
        public static string Rename(SecureString filePath, SecureString newName) => Rename(filePath.ToString(), newName.ToString());


        ///<summary> Creates a file with the specified content </summary>
        ///<param name="content"> The content of the file </param>
        ///<param name="filePath"> The path to the file </param>
        ///<param name="overwrite"> If true, the file will be overwritten if it already exists </param>
        public static void Create(string filePath, string content = "", bool overwrite = false) {

            if (!overwrite && System.IO.File.Exists(filePath)) throw new IOException("File already exists");

            using(var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write)){
                var info = new UTF8Encoding(true).GetBytes(content);
                fileStream.Write(info, 0, info.Length);
            }
        }

        ///<summary> Creates a file with the specified content </summary>
        ///<param name="content"> The content of the file </param>
        ///<param name="filePath"> The path to the file </param>
        ///<param name="overwrite"> If true, the file will be overwritten if it already exists </param>
        public static void Create(SecureString filePath, SecureString? content = null, bool overwrite = false) => Create(filePath.ToString(), content?.ToString() ?? "", overwrite);


        ///<summary> Deletes a file </summary>
        ///<param name="filePath"> The path to the file </param>
        public static void Delete(string filePath) {
            if (!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File not found");
            System.IO.File.Delete(filePath);
        }

        ///<summary> Deletes a file </summary>
        ///<param name="filePath"> The path to the file </param>
        public static void Delete(SecureString filePath) => Delete(filePath.ToString());


        ///<summary> Checks if a file exists </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<returns> True if the file exists, false otherwise </returns>
        public static bool Exists(string filePath) => System.IO.File.Exists(filePath);

        ///<summary> Checks if a file exists </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<returns> True if the file exists, false otherwise </returns>
        public static bool Exists(SecureString filePath) => Exists(filePath.ToString());


        ///<summary> Returns the number of lines in a file </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<returns> The number of lines in the file </returns>
        public static int NumberOfLines(string filePath){
            if(!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File not found");
            return System.IO.File.ReadAllLines(filePath).Length;
        }

        ///<summary> Returns the number of lines in a file </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<returns> The number of lines in the file </returns>
        public static SecureInt NumberOfLinesSecure(string filePath){
            if(!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File not found");
            return new SecureInt(System.IO.File.ReadAllLines(filePath).Length);
        }

    }
}
