using ElectroShop.Shared.Domain;
using System.Text.Json;

namespace ElectroShop.App.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProducts(int top);
    Task<IEnumerable<Product>> GetProducts();
    Task<Product?> GetProduct(int productId);
}

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;

    public ProductService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<IEnumerable<Product>> GetProducts(int top)
    {
        var products = await GetProducts();
        return products.Take(top);
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        var products = await JsonSerializer.DeserializeAsync<IEnumerable<Product>>(
            await _httpClient.GetStreamAsync($"api/product"), 
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
        );
        if (products == null) return new List<Product>();

        return products;
    }

    public async Task<Product?> GetProduct(int productId)
    {
        if (productId == 0) return null;

        var product = await JsonSerializer.DeserializeAsync<Product>(
            await _httpClient.GetStreamAsync($"api/product/{productId}"),
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
        );

        return product;
    }
}
