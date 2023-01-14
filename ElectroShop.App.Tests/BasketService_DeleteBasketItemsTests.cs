using ElectroShop.App.Helpers;
using ElectroShop.App.Services;
using ElectroShop.Shared.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace ElectroShop.App.Tests;

public class BasketService_DeleteBasketItemsTests
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
    public async Task DeleteBasketItem_Returns_True()
    {
        // Arrange
        var basketItemId = _correctBasketItem.BasketItemId;
        await MockUserCorrectBasketItem();

        // Act
        var result = await _basketService.DeleteBasketItem(basketItemId);

        // Assert
        Assert.That(result, Is.True);
    }

    private async Task MockUserCorrectBasketItem()
    {
        var mockedResponse = new HttpResponseMessage(HttpStatusCode.OK);
        _httpClientMock
            .Setup(h => h.DeleteAsync(_requestUri))
            .ReturnsAsync(mockedResponse);

        var streamMock = await mockedResponse.Content.ReadAsStreamAsync();
        _jsonHelperMock
            .Setup(r => r.GetDeserializedContentAsync<BasketItem>(streamMock))
            .ReturnsAsync(_correctBasketItem);
    }

    [Test]
    public async Task DeleteBasketItem_ZeroBasketItemId_Returns_False()
    {
        // Act
        var result = await _basketService.DeleteBasketItem(0);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteBasketItem_FailedStatusCode_Returns_False()
    {
        // Arrange
        var basketItemId = _correctBasketItem.BasketItemId;
        MockDeleteBasketItemFailedStatusCode();

        // Act
        var result = await _basketService.DeleteBasketItem(basketItemId);

        // Assert
        Assert.That(result, Is.False);
    }

    private void MockDeleteBasketItemFailedStatusCode()
    {
        var mockedResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
        _httpClientMock
            .Setup(h => h.DeleteAsync(_requestUri))
            .ReturnsAsync(mockedResponse);
    }
}
