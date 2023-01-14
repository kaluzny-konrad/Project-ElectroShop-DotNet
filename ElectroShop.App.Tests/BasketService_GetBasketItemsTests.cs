using ElectroShop.App.Helpers;
using ElectroShop.App.Services;
using ElectroShop.Shared.Domain;
using Microsoft.Extensions.Logging;
using Moq;

namespace ElectroShop.App.Tests;

public class BasketService_GetBasketItemsTests
{
    private Mock<ILogger<BasketService>> _loggerMock;
    private Mock<IJsonSerializeHelper> _jsonHelperMock;
    private Mock<IHttpClientHelper> _httpClientMock;
    private IBasketService _basketService;
    private MemoryStream _streamMock = new();

    private readonly int _userId = 1;

    private readonly List<BasketItem> _correctBasketItems = new()
    {
        new BasketItem() { BasketItemId = 1, ProductId = 139108825, Amount = 1, UserId = 1 },
        new BasketItem() { BasketItemId = 2, ProductId = 139108829, Amount = 1, UserId = 1 },
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

        var requestUri = $"api/basketItems/{_userId}";
        _httpClientMock
            .Setup(h => h.GetStreamAsync(requestUri))
            .ReturnsAsync(_streamMock);
    }

    [Test]
    public async Task GetBasketItems_Returns_BasketItems()
    {
        // Arrange
        _jsonHelperMock
            .Setup(r => r.GetDeserializedContentAsync<List<BasketItem>>(_streamMock))
            .ReturnsAsync(_correctBasketItems);

        // Act
        var result = await _basketService.GetBasketItems(_userId);

        // Assert
        Assert.That(result, Is.EqualTo(_correctBasketItems));
    }

    [Test]
    public async Task GetBasketItems_EmptyBasketItems_Returns_EmptyListAndError()
    {
        // Arrange
        _jsonHelperMock
            .Setup(r => r.GetDeserializedContentAsync<List<BasketItem>>(_streamMock))
            .ReturnsAsync(new List<BasketItem>());

        // Act
        var result = await _basketService.GetBasketItems(_userId);

        // Assert
        Assert.That(result, Is.EqualTo(new List<BasketItem>()));
    }

    [Test]
    public async Task GetBasketItems_NullBasketItems_Returns_EmptyListAndError()
    {
        // Arrange
        _jsonHelperMock
            .Setup(r => r.GetDeserializedContentAsync<List<BasketItem>>(_streamMock))
            .ReturnsAsync(value: null);

        // Act
        var result = await _basketService.GetBasketItems(_userId);

        // Assert
        Assert.That(result, Is.EqualTo(new List<BasketItem>()));
    }
}
