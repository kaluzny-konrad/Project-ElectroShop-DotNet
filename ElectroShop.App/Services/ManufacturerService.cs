using ElectroShop.Shared.Domain;
using System.Text.Json;

namespace ElectroShop.App.Services;

public interface IManufacturerService
{
    Task<Manufacturer> GetManufacturer(int manufacturerId);
}

public class ManufacturerService : IManufacturerService
{
    private readonly ILogger<ManufacturerService> _logger;
    private readonly HttpClient _httpClient;

    public ManufacturerService(ILogger<ManufacturerService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<Manufacturer> GetManufacturer(int manufacturerId)
    {
        var manufacturer = await JsonSerializer.DeserializeAsync<Manufacturer>(
            await _httpClient.GetStreamAsync($"api/manufacturer/{manufacturerId}"), 
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
        );

        if (manufacturer == null)
        {
            _logger.LogError("Manufacturer is null: {ManufacturerId}", manufacturerId);
            return new Manufacturer();
        }
        
        return manufacturer;
    }
}
