using DotNetWebInterface.Server; 

namespace DotNetWebInterface.Controllers.UserFiles
{
    public class Files
    {
        internal HttpContext? Context; 

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

        public void UnLink(string path, Action<string> OnError, Action OnSuccess) 
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
    }
}
