using Castle.Core.Resource;
using ElectroShop.App.Helpers;
using ElectroShop.App.Services;
using ElectroShop.Shared.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ElectroShop.App.Tests;

public class BasketServiceTests
{
    private Mock<ILogger<BasketService>> _loggerMock;
    private Mock<IJsonSerializeHelper> _jsonHelperMock;
    private Mock<IHttpClientHelper> _httpClientMock;
    private IBasketService _basketService;

    private readonly List<BasketItem> _correctBasketItems = new()
    {
        new BasketItem() { BasketItemId = 1, ProductId = 139108825, Amount = 1, UserId = 1 },
        new BasketItem() { BasketItemId = 2, ProductId = 139108829, Amount = 1, UserId = 1 },
    };

    private readonly List<Product> _correctProducts = new()
    {
        new Product() {
            ProductId = 139108825,
            ProductName = "Motorola Edge 30 Neo 8/128GB Czarny",
            Price = 1749M,
            ManufacturerId = 1,
        },
        new Product() {
            ProductId = 139108829,
            ProductName = "Motorola Edge 30 Neo 8/128GB Fioletowy",
            Price = 1749M,
            ManufacturerId = 1,
        },
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
    }

    [Test]
    public async Task GetBasketItems_Returns_BasketItems()
    {
        // Arrange
        var userId = 1;
        MockUserCorrectBasketData(userId);

        // Act
        var result = await _basketService.GetBasketItems(userId);

        // Assert
        Assert.That(result, Is.EqualTo(_correctBasketItems));
    }

    private void MockUserCorrectBasketData(int userId)
    {
        var requestUri = $"api/basketItems/{userId}";

        var streamMock = new MemoryStream();
        _httpClientMock
            .Setup(h => h.GetStreamAsync(requestUri))
            .ReturnsAsync(streamMock);

        _jsonHelperMock
            .Setup(r => r.GetDeserializedContentAsync<List<BasketItem>>(streamMock))
            .ReturnsAsync(_correctBasketItems);
    }
}
