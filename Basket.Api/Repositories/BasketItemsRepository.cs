using Basket.Api.Context;
using ElectroShop.Shared.Domain;

namespace Basket.Api.Repositories
{
    public interface IBasketItemsRepository
    {
        IEnumerable<BasketItem> GetBasketItems(int userId);
        BasketItem? GetBasketItem(int basketItemId);
        bool IsBasketItemExists(BasketItem basketItem);
        BasketItem AddBasketItem(BasketItem basketItem);
        BasketItem? UpdateBasketItem(BasketItem basketItem);
        void DeleteBasketItem(int basketItemId);
    }

    public class BasketItemsRepository : IBasketItemsRepository
    {
        private readonly ApiDbContext _dbContext;

        public BasketItemsRepository(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<BasketItem> GetBasketItems(int userId) 
            => _dbContext.BasketItems
                .Where(i => i.UserId == userId);

        public BasketItem? GetBasketItem(int basketItemId) 
            => _dbContext.BasketItems
                .FirstOrDefault(i => i.BasketItemId == basketItemId);

        public bool IsBasketItemExists(BasketItem basketItem)
            => _dbContext.BasketItems
                .Where(i => i.UserId == basketItem.UserId 
                    && i.ProductId == basketItem.ProductId)
                .Any();

        public BasketItem AddBasketItem(BasketItem basketItem)
        {
            var addedEntity = _dbContext.BasketItems.Add(basketItem);
            _dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public BasketItem? UpdateBasketItem(BasketItem basketItem)
        {
            var foundBasketItem = GetBasketItem(basketItem.BasketItemId);
            if (foundBasketItem == null) return null;

            foundBasketItem.Amount = basketItem.Amount;

            if (foundBasketItem.Amount <= 0)
            {
                DeleteBasketItem(foundBasketItem.BasketItemId);
                return null;
            }
            else
            {
                _dbContext.SaveChanges();
                return foundBasketItem;
            }
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
