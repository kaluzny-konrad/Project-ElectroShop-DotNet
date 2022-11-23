using ElectroShop.Shared.Domain;
using System.Text.Json;

namespace ElectroShop.App.Services
{
    public interface IProductDescriptionService
    {
        Task<string> GetShortDescription(int productId);
    }

    public class ProductDescriptionService : IProductDescriptionService
    {
        private readonly ILogger<ProductDescriptionService> _logger;
        private readonly HttpClient _httpClient;

        public ProductDescriptionService(ILogger<ProductDescriptionService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<string> GetShortDescription(int productId)
        {
            var shortDescription = await JsonSerializer.DeserializeAsync<ProductDescription>(
                await _httpClient.GetStreamAsync($"api/productdescription/{productId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
            );

            if (shortDescription == null)
            {
                _logger.LogError("Short description in productId: {ProductId}", productId);
                return string.Empty;
            }

            return shortDescription.ProductShortDescription;
        }
    }
}
