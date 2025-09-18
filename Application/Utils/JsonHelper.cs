using Domain.Models.Contents;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace ReelfyAPI.Utils
{
    public class JsonHelper
    {
        public static string SerializeJson(List<Content> movies)
        {
            try
            {
                if (movies.IsNullOrEmpty())
                {
                    throw new ArgumentNullException();
                }
                return JsonSerializer.Serialize(movies);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível transformar o Json em string.");
            }
        }



        public static List<Content> DeserializeJson(string json)
        {
            try
            {
                if (json == null)
                {
                    return new List<Content>();
                }

                return JsonSerializer.Deserialize<List<Content>>(json);
            }
            catch (JsonException e)
            {
                throw new Exception($"Error {e.Message}");
            }
        }
    }
}
