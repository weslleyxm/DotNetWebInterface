namespace DotNetWebInterface.Config
{
    public class Config
    {
        public static string GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key) ?? "";
        }
    }
}