using DotNetWebInterface.Server;
using System.Text.RegularExpressions;

namespace DotNetWebInterface.Middleware
{
    public class SqlInjectionMiddleware
    {
        private static readonly Regex SqlPattern = new(@"(?i)(--|;|/\*|\*/|@@|@|char|nchar|varchar|nvarchar|alter|begin|cast|create|cursor|declare|delete|drop|end|exec|execute|fetch|insert|kill|open|select|sys|sysobjects|syscolumns|table|update|union|or\s|and\s)", RegexOptions.Compiled);
        private static readonly Regex LogicalPattern = new(@"\b(\d+=\d+|'[^']+'='[^']+')\b", RegexOptions.Compiled);

        public Func<HttpContext, Func<Task>, Task> Invoke()
        {
            return async (context, next) =>
            {
                if (context.QueryString != null && context.QueryString.HasKeys())
                {
                    Parallel.ForEach(context.QueryString.AllKeys, key =>
                    {
                        string? rawValue = context.QueryString[key];

                        if (rawValue != null && ContainsPattern(rawValue))
                        {
                            lock (context.QueryString)
                            {
                                context.QueryString.Remove(key);
                            }
                        }
                    });
                }


                await next();
            };
        }
         
        private bool ContainsPattern(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            return SqlPattern.IsMatch(input) || LogicalPattern.IsMatch(input);
        }
    }
}
