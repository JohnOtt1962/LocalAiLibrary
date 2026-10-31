using System.Text.Json;

namespace LocalAiLibrary.AiLibrary.Helpers
{
    public class DeserializerHelper
    {
        public static T? DeserializeItem<T>(string jsonString, JsonSerializerOptions? options = null) where T : class
        {
            T? returnObject = null;

            try
            {
                returnObject = JsonSerializer.Deserialize<T>(jsonString, options);
            }
            catch (Exception ex)
            {
                // Do nothing
            }

            return returnObject;
        }
    }
}