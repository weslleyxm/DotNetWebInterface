using DotNetWebInterface.Server;
using Newtonsoft.Json;
using System.Text;

namespace DotNetWebInterface
{
    /// <summary>
    /// Provides methods to resolve HTTP requests based on route information and context
    /// </summary>
    public static class RequestResolver
    {
        /// <summary>
        /// Resolves the request based on the route information and HTTP context
        /// </summary>
        /// <param name="routeInfo">The route information</param>
        /// <param name="context">The HTTP context.</param>
        /// <returns>An array of objects representing the resolved request, or null if the request type is object</returns>
        public static object?[]? Resolver(RouteInfo routeInfo, HttpContext context)
        {
            if (routeInfo.RequestType != typeof(object))
            {
                string str = BodyToJson(context);
                var request = JsonConvert.DeserializeObject(str, routeInfo.RequestType);
                return new object?[] { request };
            }

            return null;
        }

        /// <summary>
        /// Converts the body of the HTTP request to a JSON string
        /// </summary>
        /// <param name="context">The HTTP context</param>
        /// <returns>A JSON string representing the body of the request</returns>
        private static string BodyToJson(HttpContext context)
        {
            if (!context.Request.HasEntityBody)
            {
                return "null";
            }

            using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                string body = reader.ReadToEnd().Trim();

                if (context.Parameters != null && context.Parameters.Count > 0)
                {
                    var stringBuilder = new StringBuilder(body);

                    if (string.IsNullOrWhiteSpace(body) || body.Trim(new[] { ' ', '\n', '\r' }) == "{}")
                    {
                        stringBuilder.Clear();
                        stringBuilder.Append('{');
                    }
                    else if (body.TrimEnd().EndsWith("}"))
                    {
                        stringBuilder.Remove(body.LastIndexOf('}'), 1);
                    }

                    bool firstParameter = stringBuilder.ToString().Trim() == "{";

                    foreach (var kvp in context.Parameters)
                    {
                        if (!firstParameter)
                        {
                            stringBuilder.Append(", ");
                        }
                        else
                        {
                            firstParameter = false;
                        }

                        stringBuilder.Append($"\"{kvp.Key}\": \"{kvp.Value}\"");
                    }

                    stringBuilder.Append(" }");
                    body = stringBuilder.ToString();
                }

                return body;
            }
        }
    }
}
