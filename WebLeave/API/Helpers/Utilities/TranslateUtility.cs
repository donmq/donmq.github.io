using System.Text.Json;

namespace API.Helpers.Utilities
{
    public static class TranslateUtility
    {
        /// <summary>
        /// TranslateText - Translate text by API Google
        /// </summary>
        /// <param name="text">Value translate</param>
        /// <param name="input">Language input</param>
        /// <param name="output">Language output</param>
        /// <returns>String value has been translate</returns>
        public static string TranslateText(string text, string input = "en-us", string output = "vi")
        {
            // Set the language from/to in the url (or pass it into this function)
            string url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}", input, output, Uri.EscapeDataString(text));
            HttpClient httpClient = new();
            string result = httpClient.GetStringAsync(url).Result;

            return result.Replace("[", "").Replace("]", "").Replace("\"", "").Split(",")[0];

            // Get all json data
            // var jsonData = JsonSerializer.Deserialize<List<JsonElement>>(result);

            // Translation Data
            // string translation = RecursiveJson(jsonData[0]);

            // Return translation
            // return translation;
        }

        private static string RecursiveJson(JsonElement json)
        {
            if (json.ValueKind == JsonValueKind.Array)
                return RecursiveJson(json[0]);

            return json.GetString();
        }
    }
}
