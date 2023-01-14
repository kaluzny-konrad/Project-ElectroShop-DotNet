using ElectroShop.Api.Controllers;
using ElectroShop.Api.Repositories;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ElectroShop.Api.Tests;

public class ManufacturerControllerTests
{
    private Mock<IManufacturerRepository> _repositoryMock;
    private Mock<ILogger<ManufacturerController>> _loggerMock;
    private ManufacturerController _controller;

    private readonly List<Manufacturer> _correctManufacturers = new()
    {
        new Manufacturer() {
            ManufacturerId = 1,
            ManufacturerName = "Motorola"
        },
        new Manufacturer() {
            ManufacturerId = 2,
            ManufacturerName = "Apple"
        },
    };

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new();
        _loggerMock = new();
        _controller = new ManufacturerController(
            _repositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public void GetManufacturers_IfManufacturersExists_Returns_OkResult()
    {
        // Arrange
        MockGetAllManufacturers();

        // Act
        var result = _controller.GetManufacturers();

        // Assert
        Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
    }

    [Test]
    public void GetManufacturers_IfManufacturersExists_Returns_AllManufacturers()
    {
        // Arrange
        MockGetAllManufacturers();

        // Act
        var result = _controller.GetManufacturers() as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.EqualTo(_correctManufacturers));
    }

    private void MockGetAllManufacturers()
    {
        _repositoryMock
            .Setup(r => r.GetManufacturers())
            .Returns(_correctManufacturers);
    }

    [Test]
    public void GetManufacturers_IfManufacturersNotExists_Returns_OkResult()
    {
        // Arrange
        MockGetNoManufacturers();

        // Act
        var result = _controller.GetManufacturers();

        // Assert
        Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
    }

    [Test]
    public void GetManufacturers_IfManufacturersNotExists_Returns_EmptyList()
    {
        // Arrange
        MockGetNoManufacturers();

        // Act
        var result = _controller.GetManufacturers() as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.EqualTo(new List<Manufacturer>()));
    }

    private void MockGetNoManufacturers()
    {
        _repositoryMock
            .Setup(r => r.GetManufacturers())
            .Returns(new List<Manufacturer>());
    }

    [Test]
    public void GetManufacturer_IfManufacturerIdMoreThanZero_Returns_OkResult()
    {
        // Arrange
        var manufacturerId = 1;
        MockGetFirstManufacturer(manufacturerId);

        // Act
        var result = _controller.GetManufacturer(manufacturerId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
    }

    [Test]
    public void GetManufacturer_IfManufacturerIdMoreThanZero_Returns_Manufacturer()
    {
        // Arrange
        var manufacturerId = 1;
        MockGetFirstManufacturer(manufacturerId);

        // Act
        var result = _controller.GetManufacturer(manufacturerId) as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.EqualTo(_correctManufacturers.First()));
    }

    private void MockGetFirstManufacturer(int manufacturerId)
    {
        _repositoryMock
            .Setup(r => r.GetManufacturer(manufacturerId))
            .Returns(_correctManufacturers.First());
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GetManufacturer_IfManufacturerIdLessOrEqualToZero_Returns_BadRequestResult(int manufacturerId)
    {
        // Arrange
        MockGetNoManufacturer(manufacturerId);

        // Act
        var result = _controller.GetManufacturer(manufacturerId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GetManufacturer_IfManufacturerIdLessOrEqualToZero_Returns_Null(int manufacturerId)
    {
        // Arrange
        MockGetNoManufacturer(manufacturerId);

        // Act
        var result = _controller.GetManufacturer(manufacturerId) as OkObjectResult;

        // Assert
        Assert.That(result?.Value, Is.Null);
    }

    [Test]
    public void GetManufacturer_IfManufacturerNotFound_Returns_NotFoundResult()
    {
        // Arrange
        var manufacturerId = 1;
        MockGetNoManufacturer(manufacturerId);

        // Act
        var result = _controller.GetManufacturer(manufacturerId);

        // Assert
        Assert.That(result, Is.TypeOf(typeof(NotFoundResult)));
    }

    private void MockGetNoManufacturer(int manufacturerId)
    {
        _repositoryMock
            .Setup(r => r.GetManufacturer(manufacturerId))
            .Returns(value: null);
    }
}