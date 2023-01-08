using ElectroShop.App.Controllers;
using ElectroShop.App.Services;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ElectroShop.App.Tests;

public class HomeControllerTests
{
    private Mock<ILogger<HomeController>> _loggerMock;
    private Mock<IProductService> _productServiceMock;
    private Mock<IUrlHelper> _urlHelperMock;
    private HomeController _controller;

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
        new Product() {
            ProductId = 113375066,
            ProductName = "Motorola Edge 20 8/128GB Czarny",
            Price = 1749M,
            ManufacturerId = 1,
        },
    };

    [SetUp]
    public void Setup()
    {
        _loggerMock = new();
        _productServiceMock = new();
        _urlHelperMock = new();
        _controller = new(
        _loggerMock.Object,
        _productServiceMock.Object) {
            Url = _urlHelperMock.Object
        };
    }

    [Test]
    public async Task Index_Returns_ViewResult()
    {
        // Arrange
        MockCorrectProductsData(productsInView: 3);

        // Act
        var result = await _controller.Index();

        // Assert
        Assert.That(result, Is.TypeOf(typeof(ViewResult)));
    }

    [Test]
    public async Task Index_Returns_ViewResultName_Index()
    {
        // Arrange
        var expectedViewName = "Index";
        MockCorrectProductsData(productsInView: 3);

        // Act
        var result = await _controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.That(result.ViewName, Is.EqualTo(expectedViewName));
    }

    private void MockCorrectProductsData(int productsInView)
    {
        _productServiceMock
            .Setup(r => r.GetProducts(productsInView))
            .ReturnsAsync(_correctProducts.Take(productsInView));

        foreach (var correctProduct in _correctProducts)
        {
            var imageUrl = $"~/images/product/{correctProduct.ProductId}-thumb.webp";
            _urlHelperMock
                .Setup(u => u.Content(imageUrl))
                .Returns(imageUrl);
        }
    }

    [Test]
    public void Privacy_Returns_ViewResult()
    {
        // Act
        var result = _controller.Privacy();

        // Assert
        Assert.That(result, Is.TypeOf(typeof(ViewResult)));
    }

    [Test]
    public void Privacy_Returns_ViewResultName_Privacy()
    {
        // Arrange
        var expectedViewName = "Privacy";

        // Act
        var result = _controller.Privacy() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.That(result.ViewName, Is.EqualTo(expectedViewName));
    }
}
