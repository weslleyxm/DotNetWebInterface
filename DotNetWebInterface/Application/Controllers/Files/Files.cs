using DotNetWebInterface.Server;

namespace DotNetWebInterface.Controllers.UserFiles
{
    /// <summary>
    /// Represents a collection of user files and provides methods to manage them
    /// </summary>
    public class Files
    {
        /// <summary>
        /// Gets or sets the HTTP context.
        /// </summary>
        internal HttpContext? Context;

        /// <summary>
        /// Gets a list of file information for the files in the context
        /// </summary>
        public IEnumerable<FileInfo>? List
        {
            get
            {
                if (Context?.Files == null) return null;

                var files = Context.Files.ToArray();
                List<FileInfo> fileInfos = new List<FileInfo>();

                for (int i = 0; i < files.Length; i++)
                {
                    string path = files[i];
                    if (File.Exists(path))
                    {
                        fileInfos.Add(new FileInfo(path));
                    }
                }

                return fileInfos;
            }
        }

        /// <summary>
        /// Deletes a file at the specified path
        /// </summary>
        /// <param name="path">The path of the file to delete</param>
        /// <param name="OnError">The action to invoke if an error occurs</param>
        /// <param name="OnSuccess">The action to invoke if the file is successfully deleted</param>
        public void UnlinkFile(string path, Action<string> OnError, Action OnSuccess)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    OnSuccess?.Invoke();
                }
                else
                {
                    OnError?.Invoke($"Error deleting file: \"{path}\" does not exist");
                }
            }
            catch (Exception e)
            {
                OnError?.Invoke($"Error deleting file: {e.Message}");
            }
        }

        /// <summary>
        /// Deletes all files in the context
        /// </summary>
        public void UnlinkAll()
        {
            if (List != null)
            {
                foreach (var file in List)
                {
                    string path = file.FullName;
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }
        }
    }
}
