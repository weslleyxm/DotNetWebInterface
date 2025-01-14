namespace DotNetWebInterface.Server
{
    /// <summary>
    /// Represents a configuration manager that stores and retrieves configuration options
    /// </summary>
    public class Configuration
    {
        private readonly Dictionary<Type, object> _configurations = new();

        /// <summary>
        /// Configures the specified options by applying the provided configuration action
        /// </summary>
        /// <typeparam name="TOptions">The type of the options to configure</typeparam>
        /// <param name="configure">The action to apply to the options</param>
        public void Configure<TOptions>(Action<TOptions> configure) where TOptions : class, new()
        {
            var options = new TOptions();
            configure(options);
            _configurations[typeof(TOptions)] = options;
        }

        /// <summary>
        /// Retrieves the configuration options of the specified type
        /// </summary>
        /// <typeparam name="TOptions">The type of the options to retrieve</typeparam>
        /// <returns>The configured options if found; otherwise, a new instance of the options</returns>
        public TOptions GetConfiguration<TOptions>() where TOptions : class, new()
        {
            return _configurations.TryGetValue(typeof(TOptions), out var options)
                ? (TOptions)options
                : new TOptions();
        }
    }

}
