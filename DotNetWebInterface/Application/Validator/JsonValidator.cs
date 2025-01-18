namespace DotNetWebInterface.Validator
{ 
    public static class JsonValidator
    {
        /// <summary>
        /// Validates if the input string is a JSON object or array.
        /// </summary>
        /// <param name="input">The string to validate.</param>
        /// <returns>True if the input is a valid JSON object or array, otherwise false.</returns>
        public static bool IsJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();
            return (input.StartsWith("{") && input.EndsWith("}")) ||
                   (input.StartsWith("[") && input.EndsWith("]"));
        }
    }
}
