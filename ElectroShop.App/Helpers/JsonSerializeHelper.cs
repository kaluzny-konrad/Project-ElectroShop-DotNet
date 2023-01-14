using System.Text.Json;
using System.Text;

namespace ElectroShop.App.Helpers
{
    public interface IJsonSerializeHelper
    {
        HttpContent GetSerializedContent<T>(T model);
        Task<T?> GetDeserializedContentAsync<T>(Stream stream);
    }

    public class JsonSerializeHelper : IJsonSerializeHelper
    {
        public HttpContent GetSerializedContent<T>(T model)
        {
            var stringContent = new StringContent(
            JsonSerializer.Serialize(model),
            Encoding.UTF8, "application/json");
            return stringContent;
        }

        public async Task<T?> GetDeserializedContentAsync<T>(Stream stream)
        {
            var result = await JsonSerializer.DeserializeAsync<T>(
                stream
            , new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
            );
            if (result == null) return default;
            return result;
        }
    }
}
