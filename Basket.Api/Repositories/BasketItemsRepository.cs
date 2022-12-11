using Basket.Api.Context;
using ElectroShop.Shared.Domain;

namespace Basket.Api.Repositories
{
    public interface IBasketItemsRepository
    {
        IEnumerable<BasketItem> GetBasketItems();
        IEnumerable<BasketItem> GetBasketItems(int userId);
        BasketItem? GetBasketItem(int basketItemId);
        BasketItem? GetBasketItem(int userId, int productId);
        BasketItem AddBasketItem(BasketItem basketItem);
        BasketItem? UpdateBasketItem(int basketItemId, int changeAmount);
        BasketItem? AddAmountToBasketItem(int basketItemId, int changeAmount);
        void DeleteBasketItem(int basketItemId);
    }

    public class BasketItemsRepository : IBasketItemsRepository
    {
        private readonly ApiDbContext _dbContext;

        public BasketItemsRepository(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<BasketItem> GetBasketItems()
        {
            return _dbContext.BasketItems;
        }

        public IEnumerable<BasketItem> GetBasketItems(int userId)
        {
            return GetBasketItems()
                .Where(basketItem => basketItem.UserId == userId);
        }

        public BasketItem? GetBasketItem(int basketItemId)
        {
            return GetBasketItems()
                .FirstOrDefault(basketItem => basketItem.BasketItemId == basketItemId);
        }

        public BasketItem? GetBasketItem(int userId, int productId)
        {
            return GetBasketItems()
                .Where(basketItem => basketItem.UserId == userId)
                .FirstOrDefault(basketItem => basketItem.ProductId == productId);
        }

        public BasketItem AddBasketItem(BasketItem basketItem)
        {
            var addedEntity = _dbContext.BasketItems.Add(basketItem);
            _dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public BasketItem? UpdateBasketItem(int basketItemId, int changeAmount)
        {
            var foundBasketItem = GetBasketItem(basketItemId);

            if (foundBasketItem != null)
            {
                foundBasketItem.Amount = changeAmount;

                if (foundBasketItem.Amount > 0)
                {
                    _dbContext.SaveChanges();
                    return foundBasketItem;
                }
                else DeleteBasketItem(foundBasketItem.BasketItemId);
            }

            return null;
        }

        public BasketItem? AddAmountToBasketItem(int basketItemId, int changeAmount)
        {
            var foundBasketItem = GetBasketItem(basketItemId);

            if (foundBasketItem != null)
            {
                foundBasketItem.Amount += changeAmount;

                if (foundBasketItem.Amount > 0)
                {
                    _dbContext.SaveChanges();
                    return foundBasketItem;
                }
                else DeleteBasketItem(foundBasketItem.BasketItemId);
            }

            return null;
        }

        public void DeleteBasketItem(int basketItemId)
        {
            var foundBasketItem = GetBasketItem(basketItemId);
            if (foundBasketItem == null) return;

            _dbContext.BasketItems.Remove(foundBasketItem);
            _dbContext.SaveChanges();
        }
    }
}
