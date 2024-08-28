using SimpleUtilities.Security.Cryptography;
using SimpleUtilities.Security.SecureInformation.Types;
using System.IO;
using System.Text;

using SecureString = SimpleUtilities.Security.SecureInformation.Types.SecureString;

namespace SimpleUtilities.Utilities.Files
{
    public static class File{

        ///<summary> Reads a file </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<returns> The file content as an array of lines </returns>
        public static string[] Read(string filePath, int[]? linesToRead = null, bool decrypt = false, bool includeLineBreaks = false, byte[]? key = null, byte[]? iv = null) {
            if (!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File not found");

            string all = System.IO.File.ReadAllText(filePath);
            string[] lines = all.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            if(includeLineBreaks) for (int i = 0; i < lines.Length - 1; i++) lines[i] = lines[i] + '\n';

            string[] selectedLines;

            if(linesToRead != null) {
                selectedLines = new string[linesToRead.Length];
                for (int i = 0; i < linesToRead.Length; i++) selectedLines[i] = lines[linesToRead[i]];
            }
            else selectedLines = lines;

            if (decrypt) {
                key ??= AES.GenerateKey(filePath, filePath);
                iv ??= AES.GenerateIV(filePath, filePath);

                for (int i = 0; i < selectedLines.Length; i++)
                    selectedLines[i] = AES.AESDecrypt(selectedLines[i], key, iv);
            }

            return selectedLines;
        }

        ///<summary> Reads a file </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<param name="decrypt"> If true, the content will be decrypted </param>
        public static SecureString[] SecureRead(string filePath, int[]? linesToRead = null, bool decrypt = false, bool includeLineBreaks = false, byte[]? key = null, byte[]? iv = null) {
            if (!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File not found");

            SecureString all = new SecureString(System.IO.File.ReadAllText(filePath));
            SecureString[] lines = all.Split(new[] { "\r\n", "\r", "\n" });
            if (includeLineBreaks) for (int i = 0; i < lines.Length - 1; i++) lines[i] = lines[i] + '\n';

            SecureString[] selectedLines;

            if (linesToRead != null) {
                selectedLines = new SecureString[linesToRead.Length];
                for (int i = 0; i < linesToRead.Length; i++) selectedLines[i] = lines[linesToRead[i]];
            }
            else selectedLines = lines;

            if (decrypt) {
                key ??= AES.GenerateKey(filePath, filePath);
                iv ??= AES.GenerateIV(filePath, filePath);

                for (int i = 0; i < selectedLines.Length; i++)
                    selectedLines[i] = AES.AESDecrypt(selectedLines[i], key, iv);
            }

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
        ///<remarks>The method does not write each string of content in a new line, if you want to write in a new line, add "\n" at the end of each string</remarks>
        public static void Write(string filePath, string[] content, bool append = true, bool createIfNotExist = true, bool encrypt = false, byte[]? key = null, byte[]? iv = null){

            if (!System.IO.File.Exists(filePath)) {
                if (createIfNotExist) Create(filePath, "", false);
                else throw new FileNotFoundException("File not found");
            }

            List<string> linesToWrite = new List<string>();

            foreach (string s in content) {

                string[] divided = s.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                for(int i = 0; i < divided.Length; i++) {
                    linesToWrite.Add(divided[i]);
                }
            }

            string[] lines = linesToWrite.ToArray();
            linesToWrite.Clear();

            if(lines.Length == 0) return;

            if (encrypt){

                key ??= AES.GenerateKey(filePath, filePath);
                iv ??= AES.GenerateIV(filePath, filePath);

                if (append) {
                    
                    string? lastLine = Read(filePath, decrypt: true, includeLineBreaks: true, key: key, iv: iv).LastOrDefault();
                    
                    if(lastLine != null) {

                        string encryptedLastLine = string.IsNullOrEmpty(lastLine) ? "" : AES.AESEncrypt(lastLine, key, iv);
                        char lastChar = lastLine.LastOrDefault();

                        if (lastChar != '\n' && lastChar != '\r') {
                            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite)) {             
                                fs.SetLength(fs.Length - encryptedLastLine.Length);
                            }

                            string firstLineToWrite = lastLine + lines[0];
                            lines[0] = firstLineToWrite;
                        }
                    }
                }

                for(int i = 0; i < lines.Length; i++) {
                    if (!string.IsNullOrEmpty(lines[i])) lines[i] = AES.AESEncrypt(lines[i], key, iv);
                    if (i != lines.Length - 1) lines[i] = lines[i] + "\n";
                }
            }

            using (StreamWriter streamWriter = new StreamWriter(filePath, append)) {
                foreach (string item in lines) {
                    streamWriter.Write(item);
                }            
            }
        }

        public static void SecureWrite(string filePath, SecureString[] content, bool append = true, bool createIfNotExist = true, bool encrypt = false, byte[]? key = null, byte[]? iv = null){


            if (!System.IO.File.Exists(filePath)) {
                if (createIfNotExist) Create(filePath, "", false);
                else throw new FileNotFoundException("File not found");
            }

            List<SecureString> linesToWrite = new List<SecureString>();

            foreach (SecureString s in content) {

                SecureString[] divided = s.Split(new[] { "\r\n", "\r", "\n" });

                for (int i = 0; i < divided.Length; i++) {
                    linesToWrite.Add(divided[i]);
                }
            }

            SecureString[] lines = linesToWrite.ToArray();
            linesToWrite.Clear();

            if (lines.Length == 0) return;

            if (encrypt) {

                key ??= AES.GenerateKey(filePath, filePath);
                iv ??= AES.GenerateIV(filePath, filePath);

                if (append) {

                    string? lastLine = Read(filePath, decrypt: true, includeLineBreaks: true, key: key, iv: iv).LastOrDefault();

                    if (lastLine != null) {

                        string encryptedLastLine = string.IsNullOrEmpty(lastLine) ? "" : AES.AESEncrypt(lastLine, key, iv);
                        char lastChar = lastLine.LastOrDefault();

                        if (lastChar != '\n' && lastChar != '\r') {
                            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite)) {
                                fs.SetLength(fs.Length - encryptedLastLine.Length);
                            }

                            SecureString firstLineToWrite = lastLine + lines[0];
                            lines[0] = firstLineToWrite;
                        }
                    }
                }

                for (int i = 0; i < lines.Length; i++) {
                    if (lines[i] is null || lines[i].Length == 0) lines[i] = AES.AESEncrypt(lines[i], key, iv);
                    if (i != lines.Length - 1) lines[i] = lines[i] + "\n";
                }
            }

            using (StreamWriter streamWriter = new StreamWriter(filePath, append)) {
                foreach (string item in lines) {
                    streamWriter.Write(item);
                }
            }
        }

         ///<summary> Renames a file </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<param name="newName"> The new name of the file (not the new path, only the new name) </param>
        ///<returns> The new path to the file </returns>
         public static string Rename(string filePath, string newName){
            if(!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File not found");

            DirectoryInfo? parentDirectory = System.IO.Directory.GetParent(filePath);
            string parentPath;

            if(parentDirectory != null) parentPath = parentDirectory.FullName; 
            else throw new IOException("Invalid path");

            string newPath = System.IO.Path.Combine(parentPath, newName);
            if(System.IO.File.Exists(newPath)) throw new IOException("File already exists");
            System.IO.File.Move(filePath, newPath);

            return newPath;
         }

        ///<summary> Creates a file with the specified content </summary>
        ///<param name="content"> The content of the file </param>
        ///<param name="filePath"> The path to the file </param>
        ///<param name="overwrite"> If true, the file will be overwritten if it already exists </param>
        public static void Create(string filePath, string content = "", bool overwrite = false) {

            if (!overwrite && System.IO.File.Exists(filePath)) throw new IOException("File already exists");

            using (FileStream fileStream = System.IO.File.Create(filePath)) {
                byte[] info = new UTF8Encoding(true).GetBytes(content);
                fileStream.Write(info, 0, info.Length);
            }
        }

        ///<summary> Deletes a file </summary>
        ///<param name="filePath"> The path to the file </param>
        public static void Delete(string filePath) {
            if (!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File not found");
            System.IO.File.Delete(filePath);
        }

        ///<summary> Checks if a file exists </summary>
        ///<param name="filePath"> The path to the file </param>
        ///<returns> True if the file exists, false otherwise </returns>
        public static bool Exists(string filePath) => System.IO.File.Exists(filePath);
    
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
