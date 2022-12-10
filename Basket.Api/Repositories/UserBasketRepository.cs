using Basket.Api.Context;
using ElectroShop.Shared.Domain;

namespace Basket.Api.Repositories
{
    public interface IUserBasketRepository
    {
        UserBasket? GetUserBasket(int userId);
    }

    public class UserBasketRepository : IUserBasketRepository
    {
        private readonly ApiDbContext _dbContext;

        public UserBasketRepository(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserBasket? GetUserBasket(int userId)
        {
            return GetUserBaskets().FirstOrDefault(b => b.UserId == userId);
        }

        private IEnumerable<UserBasket> GetUserBaskets()
        {
            return _dbContext.UserBaskets;
        }
    }
}
