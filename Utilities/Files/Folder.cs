using System.IO;

namespace SimpleUtilities.Utilities.Files
{
    public static class Folder
    {

        ///<summary> Creates a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        public static void Create(string folderPath)
        {

            if (Directory.Exists(folderPath)) throw new IOException("Folder already exists");

            Directory.CreateDirectory(folderPath);
        }

        ///<summary> Deletes a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        public static void Delete(string folderPath)
        {

            if (!Directory.Exists(folderPath)) throw new IOException("Folder does not exist");

            Directory.Delete(folderPath);
        }

        ///<summary> Renames a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="newName"> The new name of the folder (not the new path, only the new name) </param>
        ///<returns> The new path to the folder </returns>
        public static string Rename(string folderPath, string newName)
        {

            if (!Directory.Exists(folderPath)) throw new IOException("Folder does not exist");

            DirectoryInfo? parentDirectory = Directory.GetParent(folderPath);
            string parentPath;

            if (parentDirectory != null) parentPath = parentDirectory.FullName;
            else throw new IOException("Invalid path");

            string newPath = Path.Combine(parentPath, newName);

            if (Directory.Exists(newPath)) throw new IOException("Folder already exists");

            Directory.Move(folderPath, newPath);

            return newPath;
        }

        ///<summary> Moves a folder </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<param name="newPath"> The new path to the folder </param>
        public static void Move(string folderPath, string newPath)
        {

            if (!Directory.Exists(folderPath)) throw new IOException("Folder does not exist");

            if (Directory.Exists(newPath)) throw new IOException("Folder already exists");

            Directory.Move(folderPath, newPath);
        }

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

        ///<summary> Checks if a folder exists </summary>
        ///<param name="folderPath"> The path to the folder </param>
        ///<returns> True if the folder exists, false otherwise </returns>
        public static bool Exists(string folderPath){
            return Directory.Exists(folderPath);
        }
    }
}
