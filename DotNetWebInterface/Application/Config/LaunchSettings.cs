using System.Text.Json;

namespace DotNetWebInterface.Config
{
    internal static class LaunchSettings
    {
        private static JsonDocument? document;
        private static bool initialized = false;

        public static string? GetEnvironmentVariable(string key)
        {
            EnsureInitialized(); 
             
            if (document?.RootElement.TryGetProperty(key, out JsonElement value) == true)
            { 
                return value.ToString();
            }

            return null;
        }

        public static bool LaunchBrowser
        {
            get
            {
                return bool.TryParse(GetEnvironmentVariable("launchBrowser"), out var result) && result;
            }
        }

        public static string ApplicationUrl
        {
            get
            {
                return GetEnvironmentVariable("applicationUrl") ?? "";
            }
        }

        internal static void Init()
        {
            if (initialized) return;

            try
            {
                string filePath = Path.Combine(AppContext.BaseDirectory, "Properties", "launchSettings.json");
                string jsonContent = File.ReadAllText(filePath);
                document = JsonDocument.Parse(jsonContent); 
                initialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
                throw;
            }
        }

        private static void EnsureInitialized()
        {
            if (!initialized)
            {
                Init();
            }
        }
    }
}