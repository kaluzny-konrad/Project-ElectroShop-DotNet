using ElectroShop.App.Helpers;
using ElectroShop.Shared.Domain;
using Microsoft.IdentityModel.Tokens;

namespace ElectroShop.App.Services;

public interface IBasketService
{
    Task<List<BasketItem>> GetBasketItems(int userId);
    Task<BasketItem?> CreateBasketItem(BasketItem basketItem);
    Task<BasketItem?> UpdateBasketItem(BasketItem basketItem);
    Task<bool> DeleteBasketItem(int basketItemId);
}

public class BasketService : IBasketService
{
    private readonly ILogger<BasketService> _logger;
    private readonly IJsonSerializeHelper _jsonHelper;
    private readonly IHttpClientHelper _httpClient;

    public BasketService(
        ILogger<BasketService> logger, 
        HttpClient httpClient,
        IJsonSerializeHelper jsonHelper,
        IHttpClientHelper? httpClientHelper = null
    )
    {
        _logger = logger;
        if(httpClientHelper == null)
            _httpClient = new HttpClientHelper(httpClient);
        else
            _httpClient = httpClientHelper;
        _jsonHelper = jsonHelper;
    }

    public async Task<List<BasketItem>> GetBasketItems(int userId)
    {
        if (userId == 0) return new List<BasketItem>();

        var requestUri = $"api/basketItems/{userId}";
        var stream = await _httpClient.GetStreamAsync(requestUri);
        var basketItems = await _jsonHelper
            .GetDeserializedContentAsync<List<BasketItem>>(stream);

        if (basketItems == null || basketItems.IsNullOrEmpty())
        {
            _logger.LogError("Basket Items is empty: {userId}", userId);
            return new List<BasketItem>();
        }

        return basketItems;
    }

    public async Task<BasketItem?> CreateBasketItem(BasketItem basketItem)
    {
        if (basketItem == null || basketItem.ProductId == 0) return null;

        var basketItemJson = _jsonHelper.GetSerializedContent(basketItem);

        var requestUri = "api/basketitems";
        var response = await _httpClient.PostAsync(requestUri, basketItemJson);

        if (response.IsSuccessStatusCode)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            var createdBasketItem = await _jsonHelper
                .GetDeserializedContentAsync<BasketItem>(stream);
            return createdBasketItem;
        }

        return null;
    }

    public async Task<BasketItem?> UpdateBasketItem(BasketItem basketItem)
    {
        if (basketItem == null || basketItem.ProductId == 0) return null;

        var basketItemJson = _jsonHelper.GetSerializedContent(basketItem);

        var requestUri = $"api/basketitems/{basketItem.BasketItemId}";
        var response = await _httpClient.PutAsync(requestUri, basketItemJson);

        if (response.IsSuccessStatusCode)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            var updatedBasketItem = await _jsonHelper
                .GetDeserializedContentAsync<BasketItem>(stream);
            if (updatedBasketItem != null)
                return updatedBasketItem;
        }

        return null;
    }

    public async Task<bool> DeleteBasketItem(int basketItemId)
    {
        if (basketItemId == 0) return false;

        var requestUri = $"api/basketitems/{basketItemId}";
        var response = await _httpClient.DeleteAsync(requestUri);

        if (response.IsSuccessStatusCode)
        {
            var stream = await response.Content.ReadAsStreamAsync();
            var deletedBasketItem = await _jsonHelper
                .GetDeserializedContentAsync<BasketItem>(stream);
            if (deletedBasketItem != null)
                return true;
        }

        return false;
    }
}
