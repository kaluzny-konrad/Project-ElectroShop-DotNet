using Basket.Api.Controllers;
using Basket.Api.Repositories;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using System.Net;

namespace Basket.Api.Tests
{
    public class BasketItemsControllerTests
    {
        private Mock<IBasketItemsRepository> _repositoryMock;
        private BasketItemsController _controller;

        private readonly List<BasketItem> _correctBasketItems = new()
        {
            new BasketItem() { BasketItemId = 1, ProductId = 1, Amount = 1, UserId = 1 },
            new BasketItem() { BasketItemId = 2, ProductId = 2, Amount = 1, UserId = 1 },
            new BasketItem() { BasketItemId = 3, ProductId = 3, Amount = 1, UserId = 1 },
            new BasketItem() { BasketItemId = 4, ProductId = 1, Amount = 1, UserId = 2 },
            new BasketItem() { BasketItemId = 5, ProductId = 2, Amount = 1, UserId = 2 },
            new BasketItem() { BasketItemId = 6, ProductId = 3, Amount = 1, UserId = 2 },
            new BasketItem() { BasketItemId = 7, ProductId = 1, Amount = 1, UserId = 3 },
            new BasketItem() { BasketItemId = 8, ProductId = 2, Amount = 1, UserId = 3 },
        };

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new();
            _controller = new BasketItemsController(_repositoryMock.Object);
        }

        [Test]
        public void GetBasketItems_UserIdGreaterThanZero_Returns_OkResult()
        {
            // Arrange
            var userId = 1;
            var expectedBasketItems = _correctBasketItems.Where(b => b.UserId == userId);
            _repositoryMock
                .Setup(r => r.GetBasketItems(userId))
                .Returns(expectedBasketItems);

            // Act
            var result = _controller.GetBasketItems(userId);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
        }

        [Test]
        public void GetBasketItems_UserIdGreaterThanZero_Returns_AllUserItems()
        {
            // Arrange
            var userId = 1;
            var expectedBasketItems = _correctBasketItems.Where(b => b.UserId == userId);
            _repositoryMock
                .Setup(r => r.GetBasketItems(userId))
                .Returns(expectedBasketItems);

            // Act
            var result = _controller.GetBasketItems(userId) as OkObjectResult;

            // Assert
            Assert.That(result?.Value, Is.EqualTo(expectedBasketItems));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void GetBasketItems_UserIdLessOrEqualToZero_Returns_BadRequestResult(int userId)
        {
            // Act
            var result = _controller.GetBasketItems(userId);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void GetBasketItems_UserIdLessOrEqualToZero_Returns_ModelStateError(int userId)
        {
            // Act
            var result = _controller.GetBasketItems(userId) as BadRequestObjectResult;

            // Assert
            var modelStateErrors = result?.Value as SerializableError;
            Assert.Multiple(() =>
            {
                Assert.That(modelStateErrors?.Count, Is.EqualTo(1));
                Assert.That(modelStateErrors?["UserId"], Is.Not.Null);
            });
        }


        [Test]
        public void CreateBasketItem_EmptyBasketItem_Returns_BadRequestResult()
        {
            // Arrange
            var emptyBasketItem = new BasketItem();

            // Act
            var result = _controller.CreateBasketItem(emptyBasketItem);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        public void CreateBasketItem_EmptyBasketItem_Returns_ModelStateErrors()
        {
            // Arrange
            var emptyBasketItem = new BasketItem();

            // Act
            var result = _controller.CreateBasketItem(emptyBasketItem) 
                as BadRequestObjectResult;

            // Assert
            var modelStateErrors = result?.Value as SerializableError;
            Assert.Multiple(() =>
            {
                Assert.That(modelStateErrors?.Count, Is.EqualTo(3));
                Assert.That(modelStateErrors?["Amount"], Is.Not.Null);
                Assert.That(modelStateErrors?["UserId"], Is.Not.Null);
                Assert.That(modelStateErrors?["ProductId"], Is.Not.Null);
            });
        }

        [Test]
        public void CreateBasketItem_DuplicatedBasketItem_Returns_BadRequestResult()
        {
            // Arrange
            var duplicatedBasketItem = new BasketItem() { UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.IsBasketItemExists(duplicatedBasketItem))
                .Returns(true);

            // Act
            var result = _controller.CreateBasketItem(duplicatedBasketItem);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        public void CreateBasketItem_DuplicatedBasketItem_Returns_ModelStateError()
        {
            // Arrange
            var duplicatedBasketItem = new BasketItem() { UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.IsBasketItemExists(duplicatedBasketItem))
                .Returns(true);

            // Act
            var result = _controller.CreateBasketItem(duplicatedBasketItem) 
                as BadRequestObjectResult;

            // Assert
            var modelStateErrors = result?.Value as SerializableError;
            Assert.Multiple(() =>
            {
                Assert.That(modelStateErrors?.Count, Is.EqualTo(1));
                Assert.That(modelStateErrors?["Uniqueness"], Is.Not.Null);
            });
        }

        [Test]
        public void CreateBasketItem_NewBasketItem_Returns_OkResult()
        {
            // Arrange
            var newBasketItem = new BasketItem() { UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.IsBasketItemExists(newBasketItem))
                .Returns(false);

            var createdBasketItem = new BasketItem() 
                { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.AddBasketItem(newBasketItem))
                .Returns(createdBasketItem);

            // Act
            var result = _controller.CreateBasketItem(newBasketItem);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
        }

        [Test]
        public void CreateBasketItem_NewBasketItem_Returns_CreatedBasketItem()
        {
            // Arrange
            var newBasketItem = new BasketItem() { UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.IsBasketItemExists(newBasketItem))
                .Returns(false);

            var createdBasketItem = new BasketItem()
            { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.AddBasketItem(newBasketItem))
                .Returns(createdBasketItem);

            // Act
            var result = _controller.CreateBasketItem(newBasketItem) as OkObjectResult;

            // Assert
            Assert.That(result?.Value, Is.EqualTo(createdBasketItem));
        }

        [Test]
        public void UpdateBasketItem_EmptyBasketItem_Returns_BadRequestResult()
        {
            // Arrange
            var EmptyBasketItem = new BasketItem();

            // Act
            var result = _controller.UpdateBasketItem(EmptyBasketItem);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        public void UpdateBasketItem_EmptyBasketItem_Returns_ModelStateError()
        {
            // Arrange
            var EmptyBasketItem = new BasketItem();

            // Act
            var result = _controller.UpdateBasketItem(EmptyBasketItem) 
                as BadRequestObjectResult;

            // Assert
            var modelStateErrors = result?.Value as SerializableError;
            Assert.Multiple(() =>
            {
                Assert.That(modelStateErrors?.Count, Is.EqualTo(2));
                Assert.That(modelStateErrors?["BasketItemId"], Is.Not.Null);
                Assert.That(modelStateErrors?["Amount"], Is.Not.Null);
            });
        }

        [Test]
        public void UpdateBasketItem_NewBasketItem_Returns_NotFoundResult()
        {
            // Arrange
            var newBasketItem = new BasketItem() 
                { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.GetBasketItem(newBasketItem.BasketItemId))
                .Returns(value: null);

            // Act
            var result = _controller.UpdateBasketItem(newBasketItem);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(NotFoundResult)));
        }

        [Test]
        public void UpdateBasketItem_NewBasketItem_NotReturns_ModelStateError()
        {
            // Arrange
            var newBasketItem = new BasketItem()
                { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.GetBasketItem(newBasketItem.BasketItemId))
                .Returns(value: null);

            // Act
            var result = _controller.UpdateBasketItem(newBasketItem) as NotFoundObjectResult;

            // Assert
            var modelStateErrors = result?.Value as SerializableError;
            Assert.That(modelStateErrors, Is.Null);
        }

        [Test]
        public void UpdateBasketItem_ExistsBasketItem_AmountMoreThanZero_Returns_OkResult()
        {
            // Arrange
            var existsBasketItem = new BasketItem()
                { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.GetBasketItem(existsBasketItem.BasketItemId))
                .Returns(existsBasketItem);
            _repositoryMock
                .Setup(r => r.UpdateBasketItem(existsBasketItem))
                .Returns(existsBasketItem);

            // Act
            var result = _controller.UpdateBasketItem(existsBasketItem) as OkObjectResult;

            // Assert
            Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
        }

        [Test]
        public void UpdateBasketItem_ExistsBasketItem_AmountMoreThanZero_Returns_UpdatedItem()
        {
            // Arrange
            var existsBasketItem = new BasketItem()
                { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.GetBasketItem(existsBasketItem.BasketItemId))
                .Returns(existsBasketItem);
            _repositoryMock
                .Setup(r => r.UpdateBasketItem(existsBasketItem))
                .Returns(existsBasketItem);

            // Act
            var result = _controller.UpdateBasketItem(existsBasketItem) as OkObjectResult;

            // Assert
            Assert.That(result?.Value, Is.EqualTo(existsBasketItem));
        }

        [Test]
        public void UpdateBasketItem_ExistsBasketItem_AmountLessOrEqualZero_Returns_BadRequest()
        {
            // Arrange
            var existsBasketItem = new BasketItem()
            { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 0 };
            _repositoryMock
                .Setup(r => r.GetBasketItem(existsBasketItem.BasketItemId))
                .Returns(existsBasketItem);

            // Act
            var result = _controller.UpdateBasketItem(existsBasketItem);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        public void UpdateBasketItem_ExistsBasketItem_AmountLessOrEqualZero_Returns_ModelError()
        {
            // Arrange
            var existsBasketItem = new BasketItem()
            { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 0 };
            _repositoryMock
                .Setup(r => r.GetBasketItem(existsBasketItem.BasketItemId))
                .Returns(existsBasketItem);

            // Act
            var result = _controller.UpdateBasketItem(existsBasketItem) as BadRequestObjectResult;

            // Assert
            var modelStateErrors = result?.Value as SerializableError;
            Assert.Multiple(() =>
            {
                Assert.That(modelStateErrors?.Count, Is.EqualTo(1));
                Assert.That(modelStateErrors?["Amount"], Is.Not.Null);
            });
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void DeleteBasketItem_UserIdLessOrEqualToZero_Returns_BadRequestResult(int userId)
        {
            // Act
            var result = _controller.DeleteBasketItem(userId);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void DeleteBasketItem_UserIdLessOrEqualToZero_Returns_ModelStateError(int userId)
        {
            // Act
            var result = _controller.DeleteBasketItem(userId) as BadRequestObjectResult;

            // Assert
            var modelStateErrors = result?.Value as SerializableError;
            Assert.Multiple(() =>
            {
                Assert.That(modelStateErrors?.Count, Is.EqualTo(1));
                Assert.That(modelStateErrors?["BasketItemId"], Is.Not.Null);
            });
        }

        [Test]
        public void DeleteBasketItem_ExistsBasketItem_Returns_OkResult()
        {
            // Arrange
            var existsBasketItem = new BasketItem()
                { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.GetBasketItem(existsBasketItem.BasketItemId))
                .Returns(existsBasketItem);

            // Act
            var result = _controller.DeleteBasketItem(existsBasketItem.BasketItemId);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(OkObjectResult)));
        }

        [Test]
        public void DeleteBasketItem_ExistsBasketItem_Returns_DeletedObject()
        {
            // Arrange
            var existsBasketItem = new BasketItem() 
                { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.GetBasketItem(existsBasketItem.BasketItemId))
                .Returns(existsBasketItem);

            // Act
            var result = _controller.DeleteBasketItem(existsBasketItem.BasketItemId) 
                as OkObjectResult;

            // Assert
            Assert.That(result?.Value, Is.EqualTo(existsBasketItem));
        }

        [Test]
        public void DeleteBasketItem_NewBasketItem_Returns_NotFoundResult()
        {
            // Arrange
            var newBasketItem = new BasketItem() 
                { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.GetBasketItem(newBasketItem.BasketItemId))
                .Returns(value: null);

            // Act
            var result = _controller.DeleteBasketItem(newBasketItem.BasketItemId);

            // Assert
            Assert.That(result, Is.TypeOf(typeof(NotFoundResult)));
        }

        [Test]
        public void DeleteBasketItem_NewBasketItem_NotReturns_ModelStateError()
        {
            // Arrange
            var newBasketItem = new BasketItem()
                { BasketItemId = 1, UserId = 1, ProductId = 1, Amount = 1 };
            _repositoryMock
                .Setup(r => r.IsBasketItemExists(newBasketItem))
                .Returns(false);

            // Act
            var result = _controller.DeleteBasketItem(newBasketItem.BasketItemId) 
                as NotFoundObjectResult;

            // Assert
            var modelStateErrors = result?.Value as SerializableError;
            Assert.That(modelStateErrors, Is.Null);
        }
    }
}