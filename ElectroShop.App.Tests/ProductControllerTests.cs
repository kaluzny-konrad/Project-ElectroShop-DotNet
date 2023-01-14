using ElectroShop.App.Controllers;
using ElectroShop.App.Services;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ElectroShop.App.Tests;

public class ProductControllerTests
{
    private Mock<ILogger<ProductController>> _loggerMock;
    private Mock<IProductService> _productServiceMock;
    private Mock<IUrlHelper> _urlHelperMock;
    private ProductController _controller;

    private readonly Product _correctProduct = new()
    {
        ProductId = 139108825,
        ProductName = "Motorola Edge 30 Neo 8/128GB Czarny",
        Price = 1749M,
        ManufacturerId = 1,
    };

    [SetUp]
    public void Setup()
    {
        _loggerMock = new();
        _productServiceMock = new();
        _urlHelperMock = new();
        _controller = new(
            _loggerMock.Object,
            _productServiceMock.Object)
        {
            Url = _urlHelperMock.Object
        };
    }

    [Test]
    public async Task Details_Returns_ViewResult()
    {
        // Arrange
        var productId = _correctProduct.ProductId;
        MockCorrectProductData();

        // Act
        var result = await _controller.Details(productId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(ViewResult)));
    }

    [Test]
    public async Task Details_Returns_ViewResultName_Details()
    {
        // Arrange
        var productId = _correctProduct.ProductId;
        var expectedViewName = "Details";
        MockCorrectProductData();

        // Act
        var result = await _controller.Details(productId) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.That(result.ViewName, Is.EqualTo(expectedViewName));
    }

    private void MockCorrectProductData()
    {
        _productServiceMock
            .Setup(r => r.GetProduct(_correctProduct.ProductId))
            .ReturnsAsync(_correctProduct);

        var imageUrl = $"~/images/product/{_correctProduct.ProductId}-thumb.webp";
        _urlHelperMock
            .Setup(u => u.Content(imageUrl))
            .Returns(imageUrl);
    }
}
