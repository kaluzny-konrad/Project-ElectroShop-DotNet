using Basket.Api.Context;
using ElectroShop.Shared.Domain;

namespace Basket.Api.Repositories;

public interface IBasketItemsRepository
{
    IEnumerable<BasketItem> GetBasketItems(int userId);
    BasketItem? GetBasketItem(int basketItemId);
    bool IsBasketItemExists(BasketItem basketItem);
    BasketItem? AddBasketItem(BasketItem basketItem);
    BasketItem? UpdateBasketItem(BasketItem basketItem);
    bool DeleteBasketItem(int basketItemId);
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

    public BasketItem? AddBasketItem(BasketItem basketItem)
    {
        var foundBasketItem = GetBasketItem(basketItem.BasketItemId);
        if (foundBasketItem != null) return null;

        var addedEntity = _dbContext.BasketItems.Add(basketItem);
        if (_dbContext.SaveChanges() > 0)
            return addedEntity.Entity;
        else return null;
    }

    public BasketItem? UpdateBasketItem(BasketItem basketItem)
    {
        var foundBasketItem = GetBasketItem(basketItem.BasketItemId);
        if (foundBasketItem == null) return null;

        foundBasketItem.Amount = basketItem.Amount;

        if (_dbContext.SaveChanges() > 0)
            return foundBasketItem;
        else return null;
    }

    public bool DeleteBasketItem(int basketItemId)
    {
        var foundBasketItem = GetBasketItem(basketItemId);
        if (foundBasketItem == null) return false;

        _dbContext.BasketItems.Remove(foundBasketItem);
        return _dbContext.SaveChanges() > 0;
    }
}
