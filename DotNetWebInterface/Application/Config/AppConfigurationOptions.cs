namespace DotNetWebInterface.Config
{
    /// <summary>
    /// Configuration options for the application
    /// </summary>
    public class AppConfigurationOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether CORS is enabled
        /// </summary>
        public bool EnableCors { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether Multipart Support is enabled
        /// </summary>
        public bool EnableFileProcessing { get; set; } = false;

        /// <summary>
        /// Gets or sets the API prefix
        /// </summary>
        public string ApiPrefix { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether SQL Injection Countermeasures are enabled
        /// </summary>
        public bool SQLInjectionCountermeasures { get; set; } = false;
    }
}