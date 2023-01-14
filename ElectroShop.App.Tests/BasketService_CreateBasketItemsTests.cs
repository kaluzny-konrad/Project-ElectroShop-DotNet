using ElectroShop.App.Helpers;
using ElectroShop.App.Services;
using ElectroShop.Shared.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace ElectroShop.App.Tests;

public class BasketService_CreateBasketItemsTests
{
    private Mock<ILogger<BasketService>> _loggerMock;
    private Mock<IJsonSerializeHelper> _jsonHelperMock;
    private Mock<IHttpClientHelper> _httpClientMock;
    private IBasketService _basketService;

    private readonly StringContent _mockedHttpContent = new("content");
    private readonly string _requestUri = $"api/basketitems";

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
    public async Task CreateBasketItem_Returns_CreatedBasketItem()
    {
        // Arrange
        var basketItem = _correctBasketItem;
        await MockUserCorrectBasketItem(basketItem);

        // Act
        var result = await _basketService.CreateBasketItem(basketItem);

        // Assert
        Assert.That(result, Is.EqualTo(basketItem));
    }

    private async Task MockUserCorrectBasketItem(BasketItem basketItem)
    {
        var mockedResponse = new HttpResponseMessage(HttpStatusCode.OK);
        _httpClientMock
            .Setup(h => h.PostAsync(_requestUri, _mockedHttpContent))
            .ReturnsAsync(mockedResponse);

        var streamMock = await mockedResponse.Content.ReadAsStreamAsync();
        _jsonHelperMock
            .Setup(r => r.GetDeserializedContentAsync<BasketItem>(streamMock))
            .ReturnsAsync(basketItem);
    }

    [Test]
    public async Task CreateBasketItem_NullBasketItem_Returns_Null()
    {
        // Act
        var result = await _basketService.CreateBasketItem(null);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateBasketItem_WrongBasketItem_Returns_Null()
    {
        // Arrange
        var basketItem = _correctBasketItem;
        basketItem.ProductId = 0;

        // Act
        var result = await _basketService.CreateBasketItem(basketItem);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateBasketItem_FailedStatusCode_Returns_Null()
    {
        // Arrange
        var basketItem = _correctBasketItem;
        MockCreateBasketItemFailedStatusCode();

        // Act
        var result = await _basketService.CreateBasketItem(basketItem);

        // Assert
        Assert.That(result, Is.Null);
    }

    private void MockCreateBasketItemFailedStatusCode()
    {
        var mockedResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
        _httpClientMock
            .Setup(h => h.PostAsync(_requestUri, _mockedHttpContent))
            .ReturnsAsync(mockedResponse);
    }
}
