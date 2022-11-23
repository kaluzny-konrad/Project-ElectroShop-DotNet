using ElectroShop.Shared.Domain;
using System.Text.Json;

namespace ElectroShop.App.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();
    }

    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await JsonSerializer.DeserializeAsync<IEnumerable<Product>>(
                await _httpClient.GetStreamAsync($"api/product"), 
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
            );
            if (products == null) return new List<Product>();

            return products;
        }
    }
}
