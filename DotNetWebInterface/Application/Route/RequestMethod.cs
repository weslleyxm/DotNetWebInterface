namespace DotNetWebInterface.Route
{
    /// <summary>
    /// Enum representing the HTTP methods
    /// </summary>
    public enum RequestMethod
    {
        /// <summary>
        /// Represents an HTTP GET request
        /// </summary>
        Get,

        /// <summary>
        /// Represents an HTTP POST request
        /// </summary>
        Post,

        /// <summary>
        /// Represents multiple HTTP POST requests
        /// </summary>
        Posts,

        /// <summary>
        /// Represents an HTTP PUT request
        /// </summary>
        Put,

        /// <summary>
        /// Represents an HTTP DELETE request
        /// </summary>
        Delete
    }
}