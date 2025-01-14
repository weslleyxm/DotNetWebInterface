using DotNetWebInterface.Server;
using Newtonsoft.Json;
using System.Net;

namespace DotNetWebInterface.Route
{
    public static class RequestResolver
    {
        public static object?[]? Resolver(RouteInfo routeInfo, HttpContext context)
        {
            if (routeInfo.RequestType != typeof(object))
            {
                string str = BodyToJson(context.Request);
                var request = JsonConvert.DeserializeObject(str, routeInfo.RequestType);
                return new object?[] { request };
            }

            return null;
        }

        private static string BodyToJson(HttpListenerRequest request)
        {
            if (!request.HasEntityBody) return "null";

            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                string body = reader.ReadToEnd();
                return body;
            }
        }

        //private static string QueryStringToJson(NameValueCollection queryString)
        //{
        //    if (queryString == null) return "null";

        //    var queryDict = queryString.AllKeys
        //        .Where(key => key != null && queryString[key] != null)
        //        .ToDictionary(key => key!, key => queryString[key]!);

        //    return JsonConvert.SerializeObject(queryDict);
        //}
    }
}
