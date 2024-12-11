using System.IO;

namespace directoryPath{
    public static class DirectoryPath{
        public static string GetSolutionRoot()
        {
            // Start from the current directory
            string currentDirectory = AppContext.BaseDirectory;

            // Traverse up the directory tree until a marker is found
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            while (directory != null)
            {
                // Check if the .sln file exists or use any other marker
                if (Directory.GetFiles(directory.FullName, "*.xaml").Length > 0)
                {
                    return directory.FullName;
                }
                directory = directory.Parent;
            }

            throw new Exception("Solution root not found");
        }
    }
}