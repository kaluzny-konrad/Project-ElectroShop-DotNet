using Basket.Api.Controllers;
using Basket.Api.Repositories;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;

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
        public void GetBasketItems_ShouldReturn_AllUserItems()
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
            Assert.That(result, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(200));
                Assert.That(result.Value, Is.EqualTo(expectedBasketItems));
            });
        }

        [Test]
        public void CreateBasketItem_IfEmpty_ShouldReturn_BadRequest()
        {
            // Arrange
            var basketItem = new BasketItem();

            // Act
            var result = _controller.CreateBasketItem(basketItem) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void CreateBasketItem_IfIsUnique_ShouldCreate_BasketItem()
        {

        }
    }
}