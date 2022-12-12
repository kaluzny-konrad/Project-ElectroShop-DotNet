using Azure;
using ElectroShop.Shared.Domain;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace ElectroShop.App.Services
{
    public interface IBasketService
    {
        Task<List<BasketItem>> GetBasketItems(int userId);
        Task<bool> CreateBasketItem(BasketItem basketItem);
        Task<bool> UpdateBasketItem(BasketItem basketItem);
        Task<bool> DeleteBasketItem(int basketItemId);
    }

    public class BasketService : IBasketService
    {
        private readonly ILogger<BasketService> _logger;
        private readonly HttpClient _httpClient;

        public BasketService(ILogger<BasketService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<List<BasketItem>> GetBasketItems(int userId)
        {
            var basketItems = await JsonSerializer.DeserializeAsync<List<BasketItem>>(
                await _httpClient.GetStreamAsync($"api/basketItems/{userId}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
            );

            if (basketItems == null || basketItems.IsNullOrEmpty())
            {
                _logger.LogError("Basket Items is empty: {userId}", userId);
                return new List<BasketItem>();
            }

            return basketItems;
        }
        public async Task<bool> CreateBasketItem(BasketItem basketItem)
        {
            var basketItemJson = new StringContent(
                JsonSerializer.Serialize(basketItem), 
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/basketitems", basketItemJson);

            if (response.IsSuccessStatusCode)
            {
                var createdBasketItem = await JsonSerializer
                    .DeserializeAsync<BasketItem>
                    (await response.Content.ReadAsStreamAsync());
                if (createdBasketItem != null) 
                    return true;
            }

            return false;
        }

        public async Task<bool> UpdateBasketItem(BasketItem basketItem)
        {
            var basketItemJson = new StringContent(
                JsonSerializer.Serialize(basketItem),
                Encoding.UTF8, "application/json");

            var response = await _httpClient
                .PutAsync($"api/basketitems/{basketItem.BasketItemId}", basketItemJson);

            if (response.IsSuccessStatusCode)
            {
                var updatedBasketItem = await JsonSerializer
                    .DeserializeAsync<BasketItem>
                    (await response.Content.ReadAsStreamAsync());
                if (updatedBasketItem != null)
                    return true;
            }

            return false;
        }

        public async Task<bool> DeleteBasketItem(int basketItemId)
        {
            var response = await _httpClient.DeleteAsync($"api/basketitems/{basketItemId}");

            if (response.IsSuccessStatusCode)
            {
                var deletedBasketItem = await JsonSerializer
                    .DeserializeAsync<BasketItem>
                    (await response.Content.ReadAsStreamAsync());
                if (deletedBasketItem != null)
                    return true;
            }

            return false;
        }
    }
}
