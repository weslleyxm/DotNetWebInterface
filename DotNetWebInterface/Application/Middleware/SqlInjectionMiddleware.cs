using DotNetWebInterface.Application.Core;
using System.Text.RegularExpressions;

namespace DotNetWebInterface.Application.Middleware 
{ 
    public class SqlInjectionMiddleware
    {
        public Func<HttpContext, Func<Task>, Task> Invoke()
        {
            return async (context, next) =>
            {
                if (context.QueryString != null)
                {
                    if (context.QueryString.HasKeys())
                    {
                        foreach (var key in context.QueryString.AllKeys)
                        {
                            string? rawValue = context.QueryString[key];

                            if (rawValue != null && ContainsPattern(rawValue))
                            { 
                                context.QueryString.Remove(key); 
                            } 
                        }
                    }
                }

                await next();
            };
        }

        private bool ContainsPattern(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            string pattern = @"(?i)(--|;|/\*|\*/|@@|@|char|nchar|varchar|nvarchar|alter|begin|cast|create|cursor|declare|delete|drop|end|exec|execute|fetch|insert|kill|open|select|sys|sysobjects|syscolumns|table|update|union|or\s|and\s)";
            string logicalPattern = @"(?i)\b(\d+=\d+|'[^']+'='[^']+')\b";

            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase) ||
                   Regex.IsMatch(input, logicalPattern, RegexOptions.IgnoreCase);
        } 
    }
}
  