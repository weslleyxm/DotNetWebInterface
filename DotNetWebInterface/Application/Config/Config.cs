
namespace DotNetWebInterface.Environment
{
    public class Config
    {
        public static string GetEnvironmentVariable(string key) 
        {
            return System.Environment.GetEnvironmentVariable(key) ?? "";
        }
    }
}