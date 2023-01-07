using ElectroShop.Api.Controllers;
using ElectroShop.Api.Repositories;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ElectroShop.Api.Tests;

public class ProductControllerTests
{
    private Mock<IProductRepository> _repositoryMock;
    private Mock<ILogger<ProductController>> _loggerMock;
    private ProductController _controller;

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
        _repositoryMock = new();
        _loggerMock = new();
        _controller = new ProductController(
            _repositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public void GetProducts_IfProductsExists_Returns_OkResult()
    {
        // Arrange
        MockGetAllProducts();

        // Act
        var result = _controller.GetProducts();

        // Assert
        Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
    }

    [Test]
    public void GetProducts_IfProductsExists_Returns_AllProducts()
    {
        // Arrange
        MockGetAllProducts();

        // Act
        var result = _controller.GetProducts() as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.EqualTo(_correctProducts));
    }

    private void MockGetAllProducts()
    {
        _repositoryMock
            .Setup(r => r.GetProducts())
            .Returns(_correctProducts);
    }

    [Test]
    public void GetProducts_IfProductsNotExists_Returns_OkResult()
    {
        // Arrange
        MockGetNoProducts();

        // Act
        var result = _controller.GetProducts();

        // Assert
        Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
    }

    [Test]
    public void GetProducts_IfProductsNotExists_Returns_EmptyList()
    {
        // Arrange
        MockGetNoProducts();

        // Act
        var result = _controller.GetProducts() as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.EqualTo(new List<Product>()));
    }

    private void MockGetNoProducts()
    {
        _repositoryMock
            .Setup(r => r.GetProducts())
            .Returns(new List<Product>());
    }

    [Test]
    public void GetProduct_IfProductIdMoreThanZero_Returns_OkResult()
    {
        // Arrange
        var productId = 1;
        MockGetFirstProduct(productId);

        // Act
        var result = _controller.GetProduct(productId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
    }

    [Test]
    public void GetProduct_IfProductIdMoreThanZero_Returns_Product()
    {
        // Arrange
        var productId = 1;
        MockGetFirstProduct(productId);

        // Act
        var result = _controller.GetProduct(productId) as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.EqualTo(_correctProducts.First()));
    }

    private void MockGetFirstProduct(int productId)
    {
        _repositoryMock
            .Setup(r => r.GetProduct(productId))
            .Returns(_correctProducts.First());
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GetProduct_IfProductIdLessOrEqualToZero_Returns_BadRequestResult(int productId)
    {
        // Arrange
        MockGetNoProduct(productId);

        // Act
        var result = _controller.GetProduct(productId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GetProduct_IfProductIdLessOrEqualToZero_Returns_Null(int productId)
    {
        // Arrange
        MockGetNoProduct(productId);

        // Act
        var result = _controller.GetProduct(productId) as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.Null);
    }

    [Test]
    public void GetProduct_IfProductNotFound_Returns_NotFoundResult()
    {
        // Arrange
        var productId = 1;
        MockGetNoProduct(productId);

        // Act
        var result = _controller.GetProduct(productId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(NotFoundResult)));
    }

    private void MockGetNoProduct(int productId)
    {
        _repositoryMock
            .Setup(r => r.GetProduct(productId))
            .Returns(value: null);
    }
}