using ElectroShop.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Wishlist.Api.Repositories
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<WishlistElement>> GetWishlist(int userId);

        void AddEntity<T>(T model) where T : notnull;
        void DeleteEntity<T>(T model) where T : notnull;
        Task<bool> SaveAllAsync();
    }

    public class WishlistRepository : IWishlistRepository
    {
        private readonly WishlistContext _context;
        private readonly ILogger<WishlistRepository> _logger;

        public WishlistRepository(
            WishlistContext context, 
            ILogger<WishlistRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<WishlistElement>> GetWishlist(int userId)
        {
            var result = await _context.Wishlists
                .Where(w => w.UserId == userId)
                .ToListAsync();

            return result;
        }

        public void AddEntity<T>(T model) where T : notnull
        {
            _context.Add(model);
        }

        public void DeleteEntity<T>(T model) where T : notnull
        {
            _context.Remove(model);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
