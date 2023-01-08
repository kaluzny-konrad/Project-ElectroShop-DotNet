using ElectroShop.App.Controllers;
using ElectroShop.App.Services;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ElectroShop.App.Tests;

public class BasketControllerTests
{
    private Mock<ILogger<BasketController>> _loggerMock;
    private Mock<IBasketService> _basketServiceMock;
    private Mock<IProductService> _productServiceMock;
    private Mock<IUrlHelper> _urlHelperMock;
    private BasketController _controller;

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
        _basketServiceMock = new();
        _productServiceMock = new();
        _urlHelperMock = new();
        _controller = new(
            _loggerMock.Object,
            _basketServiceMock.Object,
            _productServiceMock.Object) {
            Url = _urlHelperMock.Object
        };
    }

    [Test]
    public async Task Index_BasketItemsExists_Returns_ViewResult()
    {
        // Arrange
        var userId = 1;
        MockCorrectBasketData(userId);

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.That(result, Is.TypeOf(typeof(ViewResult)));
    }

    [Test]
    public async Task Index_BasketItemsExists_Returns_ViewResultName_Index()
    {
        // Arrange
        var expectedViewName = "Index";
        var userId = 1;
        MockCorrectBasketData(userId);

        // Act
        var result = await _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.That(result.ViewName, Is.EqualTo(expectedViewName));
    }

    private void MockCorrectBasketData(int userId)
    {
        _basketServiceMock
            .Setup(r => r.GetBasketItems(userId))
            .ReturnsAsync(_correctBasketItems);

        foreach (var correctBasketItem in _correctBasketItems)
        {
            _productServiceMock
                .Setup(r => r.GetProduct(correctBasketItem.ProductId))
                .ReturnsAsync(_correctProducts.First(p =>
                    p.ProductId == correctBasketItem.ProductId));

            var imageUrl = $"~/images/product/{correctBasketItem.ProductId}-thumb.webp";
            _urlHelperMock
                .Setup(u => u.Content(imageUrl))
                .Returns(imageUrl);
        }
    }

    [Test]
    public async Task Index_BasketItemsNotExists_Returns_ViewResult()
    {
        // Arrange
        var userId = 1;
        MockNoBasketData(userId);

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.That(result, Is.TypeOf(typeof(ViewResult)));
    }

    [Test]
    public async Task Index_BasketItemsNotExists_Returns_ViewResultName_Empty()
    {
        // Arrange
        var expectedViewName = "Empty";
        var userId = 1;
        MockNoBasketData(userId);

        // Act
        var result = await _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.That(result.ViewName, Is.EqualTo(expectedViewName));
    }

    private void MockNoBasketData(int userId)
    {
        _basketServiceMock
            .Setup(r => r.GetBasketItems(userId))
            .ReturnsAsync(value: new List<BasketItem>());
    }

    [Test]
    public async Task Add_BasketItemFound_Should_IncreaseBasketItemAmount()
    {
        // Arrange
        var userId = 1;
        var productId = _correctProducts.First().ProductId;
        var amountBefore = _correctBasketItems
            .First(bi => bi.ProductId == productId).Amount;
        var expectedAmount = amountBefore + 1;
        MockCorrectBasketData(userId);

        // Act
        var result = await _controller.Add(productId);
        var amountAfter = _correctBasketItems
            .First(bi => bi.ProductId == productId).Amount;

        // Assert
        Assert.That(amountAfter, Is.EqualTo(expectedAmount));
    }

    [Test]
    public async Task Add_BasketItemFound_Returns_RedirectToAction()
    {
        // Arrange
        var userId = 1;
        var productId = _correctProducts.First().ProductId;
        MockCorrectBasketData(userId);

        // Act
        var result = await _controller.Add(productId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(RedirectToActionResult)));
    }

    [Test]
    public async Task Add_BasketItemFound_Returns_ActionName_Index()
    {
        // Arrange
        var expectedViewName = "Index";
        var userId = 1;
        var productId = _correctProducts.First().ProductId;
        MockCorrectBasketData(userId);

        // Act
        var result = await _controller.Add(productId) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.That(result.ActionName, Is.EqualTo(expectedViewName));
    }

    [Test]
    public async Task Add_BasketItemNotFound_Should_CreateBasketItem()
    {
        // Arrange
        var userId = 1;
        var productId = _correctProducts.First().ProductId;
        var expectedBasketItem = new BasketItem()
            { UserId = userId, ProductId = productId, Amount = 1 };
        MockNoBasketData(userId);

        // Act
        var result = await _controller.Add(productId);

        // Assert
        _basketServiceMock.Verify(bs => 
            bs.CreateBasketItem(It.Is<BasketItem>(
                bi =>  bi.UserId == expectedBasketItem.UserId
                    && bi.ProductId == expectedBasketItem.ProductId
                    && bi.Amount == expectedBasketItem.Amount
            ))
            , Times.Once);
    }

    [Test]
    public async Task Add_BasketItemNotFound_Returns_RedirectToAction()
    {
        // Arrange
        var userId = 1;
        var productId = _correctProducts.First().ProductId;
        MockNoBasketData(userId);

        // Act
        var result = await _controller.Add(productId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(RedirectToActionResult)));
    }

    [Test]
    public async Task Add_BasketItemNotFound_Returns_ActionName_Index()
    {
        // Arrange
        var expectedViewName = "Index";
        var userId = 1;
        var productId = _correctProducts.First().ProductId;
        MockNoBasketData(userId);

        // Act
        var result = await _controller.Add(productId) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.That(result.ActionName, Is.EqualTo(expectedViewName));
    }



    [Test]
    public async Task Delete_Should_DeleteBasketItem()
    {
        // Arrange
        var basketItemId = _correctBasketItems.First().BasketItemId;

        // Act
        var result = await _controller.Delete(basketItemId);

        // Assert
        _basketServiceMock.Verify(bs =>
            bs.DeleteBasketItem(It.Is<int>(
                biId => biId == basketItemId
            ))
            , Times.Once);
    }

    [Test]
    public async Task Delete_Returns_RedirectToAction()
    {
        // Arrange
        var basketItemId = _correctBasketItems.First().BasketItemId;

        // Act
        var result = await _controller.Delete(basketItemId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(RedirectToActionResult)));
    }

    [Test]
    public async Task Delete_Returns_ActionName_Index()
    {
        // Arrange
        var expectedViewName = "Index";
        var basketItemId = _correctBasketItems.First().BasketItemId;

        // Act
        var result = await _controller.Delete(basketItemId) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.That(result.ActionName, Is.EqualTo(expectedViewName));
    }
}