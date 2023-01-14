using ElectroShop.Api.Controllers;
using ElectroShop.Api.Repositories;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ElectroShop.Api.Tests;

public class ProductDescriptionControllerTests
{
    private Mock<IProductDescriptionRepository> _repositoryMock;
    private Mock<ILogger<ProductDescriptionController>> _loggerMock;
    private ProductDescriptionController _controller;

    private readonly List<ProductDescription> _correctProductDescriptions = new()
    {
        new ProductDescription() {
                ProductDescriptionId = 1,
                ProductId = 139108825,
                ProductShortDescription =
@"Smartfon Motorola z ekranem 6,7 cala, wyświetlacz OLED. Aparat 108 Mpix, pamięć 8 GB RAM, bateria 4000mAh. Obsługuje sieć: 5G",
                ProductLongDescription =
@"Ultrasmukły i lekki, z kinowym ekranem 6,28"", oferowany w najnowszych modnych kolorach Pantone. 
System aparatów 64 Mpx z OIS 
Rób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, 
jednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.",
            },
        new ProductDescription() {
                ProductDescriptionId = 2,
                ProductId = 139108829,
                ProductShortDescription =
@"Smartfon Motorola z ekranem 6,7 cala, wyświetlacz OLED. Aparat 108 Mpix, pamięć 8 GB RAM, bateria 4000mAh. Obsługuje sieć: 5G",
                ProductLongDescription =
@"Ultrasmukły i lekki, z kinowym ekranem 6,28"", oferowany w najnowszych modnych kolorach Pantone. 
System aparatów 64 Mpx z OIS 
Rób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, 
jednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.",
            },
    };

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new();
        _loggerMock = new();
        _controller = new ProductDescriptionController(
            _repositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public void GetProductDescriptions_IfProductDescriptionsExists_Returns_OkResult()
    {
        // Arrange
        MockGetAllProductDescriptions();

        // Act
        var result = _controller.GetProductDescriptions();

        // Assert
        Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
    }

    [Test]
    public void GetProductDescriptions_IfProductDescriptionsExists_Returns_AllProductDescriptions()
    {
        // Arrange
        MockGetAllProductDescriptions();

        // Act
        var result = _controller.GetProductDescriptions() as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.EqualTo(_correctProductDescriptions));
    }

    private void MockGetAllProductDescriptions()
    {
        _repositoryMock
            .Setup(r => r.GetProductDescriptions())
            .Returns(_correctProductDescriptions);
    }

    [Test]
    public void GetProductDescriptions_IfProductDescriptionsNotExists_Returns_OkResult()
    {
        // Arrange
        MockGetNoProductDescriptions();

        // Act
        var result = _controller.GetProductDescriptions();

        // Assert
        Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
    }

    [Test]
    public void GetProductDescriptions_IfProductDescriptionsNotExists_Returns_EmptyList()
    {
        // Arrange
        MockGetNoProductDescriptions();

        // Act
        var result = _controller.GetProductDescriptions() as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.EqualTo(new List<ProductDescription>()));
    }

    private void MockGetNoProductDescriptions()
    {
        _repositoryMock
            .Setup(r => r.GetProductDescriptions())
            .Returns(new List<ProductDescription>());
    }

    [Test]
    public void GetProductDescription_IfProductDescriptionIdMoreThanZero_Returns_OkResult()
    {
        // Arrange
        var productDescriptionId = 1;
        MockGetFirstProductDescription(productDescriptionId);

        // Act
        var result = _controller.GetProductDescription(productDescriptionId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
    }

    [Test]
    public void GetProductDescription_IfProductDescriptionIdMoreThanZero_Returns_ProductDescription()
    {
        // Arrange
        var productDescriptionId = 1;
        MockGetFirstProductDescription(productDescriptionId);

        // Act
        var result = _controller.GetProductDescription(productDescriptionId) as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.EqualTo(_correctProductDescriptions.First()));
    }

    private void MockGetFirstProductDescription(int productDescriptionId)
    {
        _repositoryMock
            .Setup(r => r.GetProductDescription(productDescriptionId))
            .Returns(_correctProductDescriptions.First());
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GetProductDescription_IfProductDescriptionIdLessOrEqualToZero_Returns_BadRequestResult(int productDescriptionId)
    {
        // Arrange
        MockGetNoProductDescription(productDescriptionId);

        // Act
        var result = _controller.GetProductDescription(productDescriptionId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GetProductDescription_IfProductDescriptionIdLessOrEqualToZero_Returns_Null(int productDescriptionId)
    {
        // Arrange
        MockGetNoProductDescription(productDescriptionId);

        // Act
        var result = _controller.GetProductDescription(productDescriptionId) as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.Null);
    }

    [Test]
    public void GetProductDescription_IfProductDescriptionNotFound_Returns_NotFoundResult()
    {
        // Arrange
        var productDescriptionId = 1;
        MockGetNoProductDescription(productDescriptionId);

        // Act
        var result = _controller.GetProductDescription(productDescriptionId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(NotFoundResult)));
    }

    private void MockGetNoProductDescription(int productDescriptionId)
    {
        _repositoryMock
            .Setup(r => r.GetProductDescription(productDescriptionId))
            .Returns(value: null);
    }
}