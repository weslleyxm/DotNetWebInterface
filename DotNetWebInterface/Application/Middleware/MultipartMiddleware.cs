using DotNetWebInterface.Controllers;
using DotNetWebInterface.Server;
using HttpMultipartParser;

namespace DotNetWebInterface.Multipart.Middleware
{
    internal class MultipartMiddleware
    {
        public Func<HttpContext, Func<Task>, Task> Invoke()
        {
            return static async (context, next) =>
            {
                // Check if the request is a POST request with multipart/form-data content type
                if (context.Request?.HttpMethod == "POST" && context.Request?.ContentType?.Contains("multipart/form-data") == true)
                {
                    try
                    {
                        // Read the input stream from the request
                        using (var stream = context.Request.InputStream)
                        {
                            // Parse the multipart form data
                            var parser = await MultipartFormDataParser.ParseAsync(stream).ConfigureAwait(false);
                             
                            //get the parameters
                            context.Parameters = parser.Parameters?.ToDictionary(
                                parameter => parameter.Name,
                                parameter => parameter.Data
                            ) ?? new Dictionary<string, string>();

                            //check if the method support multipart
                            if (ControllerResolver.SupportMultipart(context.AbsolutePath))
                            {
                                // Get the base directory of the application
                                string programDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                // Define the upload path
                                string uploadPath = Path.Combine(programDirectory, "uploads");
                                // Initialize the files list in the context
                                context.Files = new List<string>();

                                // Create the upload directory if it doesn't exist
                                if (!Directory.Exists(uploadPath))
                                {
                                    Directory.CreateDirectory(uploadPath);
                                }

                                // Iterate over the parsed files
                                foreach (var file in parser.Files)
                                {
                                    // Get the file extension
                                    string fileExtension = Path.GetExtension(file.FileName);
                                    // Generate a unique file name using GUID
                                    string guidFileName = $"{Guid.NewGuid()}{fileExtension}";
                                    // Get the full file path
                                    string fullFilePath = Path.Combine(uploadPath, guidFileName);

                                    // Save the file to the upload directory
                                    using (var fileStream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write))
                                    {
                                        await file.Data.CopyToAsync(fileStream);
                                    }

                                    // Add the file path to the context's files list
                                    context.Files = context.Files.Append(fullFilePath);
                                }
                            } 
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error message
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                }

                // Call the next middleware in the pipeline
                await next();
            };
        }
    }
}