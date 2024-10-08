using SimpleUtilities.Security.SecureInformation.Types.Numerics;
using SimpleUtilities.Security.SecureInformation.Types.Texts;

namespace SimpleUtilities.Utilities.Files
{
    public static class Folder{

        ///<summary> Creates a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        public static void Create(string folderPath){

            if (Directory.Exists(folderPath)) throw new IOException("Folder already exists");

            Directory.CreateDirectory(folderPath);
        }

        ///<summary> Creates a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        public static void Create(SecureString folderPath) => Create(folderPath.ToString());


        ///<summary> Deletes a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="deleteRecursive"> If true, deletes the folder and all its content. If false, deletes the folder only if it is empty </param>
        public static void Delete(string folderPath, bool deleteRecursive = false){

            if (!Directory.Exists(folderPath)) throw new IOException("Folder does not exist");

            Directory.Delete(folderPath, deleteRecursive);
        }

        ///<summary> Deletes a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="deleteRecursive"> If true, deletes the folder and all its content. If false, deletes the folder only if it is empty </param>
        public static void Delete(SecureString folderPath, bool deleteRecursive = false) => Delete(folderPath.ToString(), deleteRecursive);


        ///<summary> Renames a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="newName"> The new name of the folder (not the new path, only the new name) </param>
        ///<returns> The new path to the folder </returns>
        public static string Rename(string folderPath, string newName){

            if (!Directory.Exists(folderPath)) throw new IOException("Folder does not exist");

            var parentDirectory = Directory.GetParent(folderPath);
            string parentPath;

            if (parentDirectory != null) parentPath = parentDirectory.FullName;
            else throw new IOException("Invalid path");

            var newPath = Path.Combine(parentPath, newName);

            if (Directory.Exists(newPath)) throw new IOException("Folder already exists");

            Directory.Move(folderPath, newPath);

            return newPath;
        }

        ///<summary> Renames a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="newName"> The new name of the folder (not the new path, only the new name) </param>
        ///<returns> The new path to the folder </returns>
        public static SecureString Rename(SecureString folderPath, SecureString newName) => (SecureString)Rename(folderPath.ToString(), newName.ToString());


        ///<summary> Moves a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="newPath"> The new path to the folder </param>
        public static void Move(string folderPath, string newPath){

            if (!Directory.Exists(folderPath)) throw new IOException("Folder does not exist");

            if (Directory.Exists(newPath)) throw new IOException("Folder already exists");

            Directory.Move(folderPath, newPath);
        }

        ///<summary> Moves a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="newPath"> The new path to the folder </param>
        public static void Move(SecureString folderPath, SecureString newPath) => Move(folderPath.ToString(), newPath.ToString());


        ///<summary> Copies a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="newPath"> The new path to the folder </param>
        public static void Copy(string folderPath, string newPath)
        {

            if (!Directory.Exists(folderPath)) throw new IOException("Folder does not exist");

            if (Directory.Exists(newPath)) throw new IOException("Folder already exists");

            Directory.CreateDirectory(newPath);

            foreach (string file in Directory.GetFiles(folderPath))
            {
                System.IO.File.Copy(file, Path.Combine(newPath, Path.GetFileName(file)));
            }

            foreach (string folder in Directory.GetDirectories(folderPath))
            {
                Copy(folder, Path.Combine(newPath, Path.GetFileName(folder)));
            }
        }

        ///<summary> Copies a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="newPath"> The new path to the folder </param>
        public static void Copy(SecureString folderPath, SecureString newPath) => Copy(folderPath.ToString(), newPath.ToString());


        ///<summary> Checks if a folder exists </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<returns> True if the folder exists, false otherwise </returns>
        public static bool Exists(string folderPath){
            return Directory.Exists(folderPath);
        }

        ///<summary> Checks if a folder exists </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<returns> True if the folder exists, false otherwise </returns>
        public static bool Exists(SecureString folderPath) => Exists(folderPath.ToString());


        ///<summary> Gets the number of files in a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="recursive"> If true, counts all files in all subdirectories. If false, counts only the files in the given directory </param>
        public static int GetNumberOfFiles(string folderPath, bool recursive = false) {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"The directory '{folderPath}' does not exist.");

            return Directory.GetFiles(folderPath, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Length;
        }

        ///<summary> Gets the number of files in a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="recursive"> If true, counts all files in all subdirectories. If false, counts only the files in the given directory </param>
        public static SecureInt GetNumberOfFiles(SecureString folderPath, bool recursive = false) {
            return (SecureInt)GetNumberOfFiles(folderPath.ToString(), recursive);
        }


        ///<summary> Gets the number of folders in a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="recursive"> If true, counts all folders in all subdirectories. If false, counts only the folders in the given directory </param>
        public static int GetNumberOfFolders(string folderPath, bool recursive = false) {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"The directory '{folderPath}' does not exist.");

            return Directory.GetDirectories(folderPath, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Length;
        }

        ///<summary> Gets the number of folders in a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="recursive"> If true, counts all folders in all subdirectories. If false, counts only the folders in the given directory </param>
        public static SecureInt GetNumberOfFolders(SecureString folderPath, bool recursive = false) {
            return (SecureInt)GetNumberOfFolders(folderPath.ToString(), recursive);
        }


        ///<summary> Gets the first file with the given extension </summary>
        ///<param name="directoryPath"> The path to the directory </param>
        ///<param name="extension"> The extension of the file </param>
        ///<param name="recursive"> If true, searches in all subdirectories. If false, searches only in the given directory </param>
        public static string? GetFirstFileWithExtension(string directoryPath, string extension, bool recursive = true) {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");

            string[] files = Directory.GetFiles(directoryPath, $"*{extension}", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            return files.Length > 0 ? files[0] : null;
        }

        ///<summary> Gets the first file with the given extension </summary>
        ///<param name="directoryPath"> The path to the directory </param>
        ///<param name="extension"> The extension of the file </param>
        ///<param name="recursive"> If true, searches in all subdirectories. If false, searches only in the given directory </param>
        public static SecureString? GetFirstFileWithExtension(SecureString directoryPath, SecureString extension, bool recursive = true) {
            string? s = GetFirstFileWithExtension(directoryPath.ToString(), extension.ToString(), recursive);

            return s != null ? new SecureString(s) : null;
        }


        ///<summary> Gets all files with the given extension </summary>
        ///<param name="directoryPath"> The path to the directory </param>
        ///<param name="extension"> The extension of the files </param>
        ///param name="recursive"> If true, searches in all subdirectories. If false, searches only in the given directory </param>
        ///<returns> An array with all the files with the given extension </returns>
        public static string[] GetAllFilesWithExtension(string directoryPath, string extension, bool recursive = true) {
            if(!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");

            return Directory.GetFiles(directoryPath, $"*{extension}", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);     
        }

        ///<summary> Gets all files with the given extension </summary>
        ///<param name="directoryPath"> The path to the directory </param>
        ///<param name="extension"> The extension of the files </param>
        ///<param name="recursive"> If true, searches in all subdirectories. If false, searches only in the given directory </param>
        ///<returns> An array with all the files with the given extension </returns>
        public static SecureString[] GetAllFilesWithExtension(SecureString directoryPath, SecureString extension, bool recursive = true) {
            string[] files = GetAllFilesWithExtension(directoryPath.ToString(), extension.ToString(), recursive);

            SecureString[] secureFiles = new SecureString[files.Length];

            for (int i = 0; i < files.Length; i++)
                secureFiles[i] = new SecureString(files[i]);

            return secureFiles;
        }


        ///<summary> Gets all files in a folder </summary>
        ///<param name="directoryPath"> The path to the directory </param>
        ///<param name="recursive"> If true, searches in all subdirectories. If false, searches only in the given directory </param>
        ///<returns> An array with all the full paths to the files </returns>
        public static string[] GetAllFiles(string directoryPath, bool recursive = true) {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");

            return Directory.GetFiles(directoryPath, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        ///<summary> Gets all files in a folder </summary>
        ///<param name="directoryPath"> The path to the directory </param>
        ///<param name="recursive"> If true, searches in all subdirectories. If false, searches only in the given directory </param>
        ///<returns> An array with all the full paths to the files </returns>
        public static SecureString[] GetAllFiles(SecureString directoryPath, bool recursive = true) {
            string[] files = GetAllFiles(directoryPath.ToString(), recursive);

            SecureString[] secureFiles = new SecureString[files.Length];

            for (int i = 0; i < files.Length; i++)
                secureFiles[i] = new SecureString(files[i]);

            return secureFiles;
        }


        ///<summary> Gets all folders in a folder </summary>
        ///<param name="directoryPath"> The path to the directory </param>
        ///<param name="recursive"> If true, searches in all subdirectories. If false, searches only in the given directory </param>
        ///<returns> An array with all the full paths to the folders </returns>
        public static string[] GetAllFolders(string directoryPath, bool recursive = true) {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");

            return Directory.GetDirectories(directoryPath, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        ///<summary> Gets all folders in a folder </summary>
        ///<param name="directoryPath"> The path to the directory </param>
        ///<param name="recursive"> If true, searches in all subdirectories. If false, searches only in the given directory </param>
        ///<returns> An array with all the full paths to the folders </returns>
        public static SecureString[] GetAllFolders(SecureString directoryPath, bool recursive = true) {
            string[] folders = GetAllFolders(directoryPath.ToString(), recursive);

            SecureString[] secureFolders = new SecureString[folders.Length];

            for (int i = 0; i < folders.Length; i++)
                secureFolders[i] = new SecureString(folders[i]);

            return secureFolders;
        }


        ///<summary> Checks if a folder is empty </summary>
        ///<param name="folderPath"> The path to the folder </param>
        public static bool IsEmpty(string folderPath) {
            return Directory.GetFiles(folderPath).Length == 0 && Directory.GetDirectories(folderPath).Length == 0;
        }

        ///<summary> Checks if a folder is empty </summary>
        ///<param name="folderPath"> The path to the folder </param>
        public static bool IsEmpty(SecureString folderPath) => IsEmpty(folderPath.ToString());


        ///<summary> Empties the content of a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        public static void EmptyContent(string folderPath) {
            foreach (string file in Directory.GetFiles(folderPath)) System.IO.File.Delete(file);
            foreach (string folder in Directory.GetDirectories(folderPath)) Directory.Delete(folder, true);
        }

        ///<summary> Empties the content of a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        public static void EmptyContent(SecureString folderPath) => EmptyContent(folderPath.ToString());
    }
}
