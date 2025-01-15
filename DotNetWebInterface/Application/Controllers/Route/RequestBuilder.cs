using Newtonsoft.Json;
using System.Collections.Specialized;

namespace DotNetWebInterface
{
    /// <summary>
    /// Provides methods to create requests from raw JSON strings
    /// </summary>
    public static class RequestBuilder
    {
        /// <summary>
        /// Creates a request object of type T from a raw JSON string
        /// </summary>
        /// <typeparam name="T">The type of the request object</typeparam>
        /// <param name="rawRequest">The raw JSON string representing the request</param>
        /// <returns>An instance of type T if deserialization is successful; otherwise, null</returns>
        public static T? CreateRequest<T>(NameValueCollection queryString)
        {
            var request = JsonConvert.DeserializeObject<T>(QueryStringToJson(queryString));
            return request;
        }

        private static string QueryStringToJson(NameValueCollection queryString)
        {
            var queryDict = new Dictionary<string, string>();

            if (queryString == null) return "";

            foreach (string? key in queryString.AllKeys)
            {
                if (key != null && queryString[key] != null)
                {
                    queryDict[key] = queryString[key]!;
                }
            }

            return JsonConvert.SerializeObject(queryDict, Formatting.Indented);
        }
    }
}
