using Newtonsoft.Json; 

namespace DotNetWebInterface.Controllers
{
    /// <summary>
    /// Represents an abstract base class for defining controller
    /// </summary>
    public abstract class ControllerBase : Controller
    {
        /// <summary>
        /// Sends a response asynchronously with the specified status code
        /// </summary>
        /// <typeparam name="T">The type of the response object</typeparam>
        /// <param name="response">The response object to send</param>
        /// <param name="statusCode">The HTTP status code to send</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected async Task SendAsync<T>(T response, int statusCode = 200) where T : class
        {
            string message = JsonConvert.SerializeObject(response);
            await WriteAsync(message, statusCode);
        }

        /// <summary>
        /// Sends an OK response asynchronously
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected async Task SendOk(string message = "OK")
        {
            await SendResponse(message, 200); 
        } 

        /// <summary>
        /// Sends a Bad Request response asynchronously with an optional error message
        /// </summary>
        /// <param name="errorMessage">The error message to send</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected async Task SendBadRequest(string errorMessage = "Bad Request")
        {
            await SendResponse(errorMessage, 400);
        }

        /// <summary>
        /// Sends an Unauthorized response asynchronously with an optional error message
        /// </summary>
        /// <param name="errorMessage">The error message to send</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected async Task SendUnauthorized(string errorMessage = "Unauthorized")
        {
            await SendResponse(errorMessage, 401);
        }

        /// <summary>
        /// Sends a Forbidden response asynchronously with an optional error message
        /// </summary>
        /// <param name="errorMessage">The error message to send</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected async Task SendForbidden(string errorMessage = "Forbidden")
        {
            await SendResponse(errorMessage, 403);
        }

        /// <summary>
        /// Sends a Not Found response asynchronously with an optional error message
        /// </summary>
        /// <param name="errorMessage">The error message to send</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected async Task SendNotFound(string errorMessage = "Not Found")
        {
            await SendResponse(errorMessage, 404);
        }

        /// <summary>
        /// Sends an Internal Server Error response asynchronously with an optional error message
        /// </summary>
        /// <param name="errorMessage">The error message to send</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected async Task SendInternalServerError(string errorMessage = "Internal Server Error")
        {
            await SendResponse(errorMessage, 500);
        }

        /// <summary>
        /// Sends a custom error response asynchronously with the specified error message and status code
        /// </summary>
        /// <param name="errorMessage">The error message to send</param>
        /// <param name="errorCode">The HTTP status code to send</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected async Task SendCustomError(string errorMessage, int errorCode)
        {
            await SendResponse(errorMessage, errorCode);
        }

        /// <summary>
        /// Sends a response asynchronously with the specified status code
        /// </summary>
        /// <param name="response">The response message to send</param>
        /// <param name="statusCode">The HTTP status code to send</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        private async Task SendResponse(string response, int statusCode = 200)
        {
            string message = JsonConvert.SerializeObject(new
            {
                code = statusCode,
                message = response
            });

            await WriteAsync(message, statusCode);
        } 
    }
}
