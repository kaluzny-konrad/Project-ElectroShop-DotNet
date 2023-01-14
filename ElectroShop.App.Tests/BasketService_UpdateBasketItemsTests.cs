using ElectroShop.App.Helpers;
using ElectroShop.App.Services;
using ElectroShop.Shared.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace ElectroShop.App.Tests;

public class BasketService_UpdateBasketItemsTests
{
    private Mock<ILogger<BasketService>> _loggerMock;
    private Mock<IJsonSerializeHelper> _jsonHelperMock;
    private Mock<IHttpClientHelper> _httpClientMock;
    private IBasketService _basketService;

    private readonly StringContent _mockedHttpContent = new("content");
    private readonly string _requestUri = $"api/basketitems/1";

    private readonly BasketItem _correctBasketItem = new()
    {
        BasketItemId = 1, ProductId = 139108825, Amount = 1, UserId = 1
    };

    [SetUp]
    public void Setup()
    {
        _loggerMock = new();
        _httpClientMock = new();
        _jsonHelperMock = new();
        _basketService = new BasketService(
        _loggerMock.Object,
            new HttpClient(),
            _jsonHelperMock.Object,
            _httpClientMock.Object);

        
        _jsonHelperMock
            .Setup(r => r.GetSerializedContent(_correctBasketItem))
            .Returns(_mockedHttpContent);
    }

    [Test]
    public async Task UpdateBasketItem_Returns_UpdatedBasketItem()
    {
        // Arrange
        var basketItem = _correctBasketItem;
        await MockUserCorrectBasketItem();

        // Act
        var result = await _basketService.UpdateBasketItem(basketItem);

        // Assert
        Assert.That(result, Is.EqualTo(basketItem));
    }

    private async Task MockUserCorrectBasketItem()
    {
        var mockedResponse = new HttpResponseMessage(HttpStatusCode.OK);
        _httpClientMock
            .Setup(h => h.PutAsync(_requestUri, _mockedHttpContent))
            .ReturnsAsync(mockedResponse);

        var streamMock = await mockedResponse.Content.ReadAsStreamAsync();
        _jsonHelperMock
            .Setup(r => r.GetDeserializedContentAsync<BasketItem>(streamMock))
            .ReturnsAsync(_correctBasketItem);
    }

    [Test]
    public async Task UpdateBasketItem_NullBasketItem_Returns_Null()
    {
        // Act
        var result = await _basketService.UpdateBasketItem(null);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateBasketItem_WrongBasketItem_Returns_Null()
    {
        // Arrange
        var basketItem = _correctBasketItem;
        basketItem.ProductId = 0;

        // Act
        var result = await _basketService.UpdateBasketItem(basketItem);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task UpdateBasketItem_FailedStatusCode_Returns_Null()
    {
        // Arrange
        var basketItem = _correctBasketItem;
        MockUpdateBasketItemFailedStatusCode();

        // Act
        var result = await _basketService.UpdateBasketItem(basketItem);

        // Assert
        Assert.That(result, Is.Null);
    }

    private void MockUpdateBasketItemFailedStatusCode()
    {
        var mockedResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
        _httpClientMock
            .Setup(h => h.PutAsync(_requestUri, _mockedHttpContent))
            .ReturnsAsync(mockedResponse);
    }
}
