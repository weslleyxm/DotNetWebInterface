using System;
using Newtonsoft.Json;

namespace DotNetWebInterface.Validator
{ 
    public static class JsonValidator
    {
        public static bool IsJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            try
            {
                JsonConvert.DeserializeObject(input);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}