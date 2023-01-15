using ElectroShop.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Wishlist.Api.Repositories;

public interface IWishlistRepository
{
    Task<IEnumerable<WishlistElement>> GetWishlist(int userId);
    Task<WishlistElement?> GetWishlistElement(int wishlistElementId);

    Task<bool> AddWishlist(WishlistElement wishlistElement);
    Task<bool> DeleteWishlistElement(WishlistElement wishlistElement);
    Task<bool> DeleteWishlistElement(int wishlistElementId);
    Task<bool> DeleteWishlist(int userId);
    Task<int> SaveAllAsync();
}

public class WishlistRepository : IWishlistRepository
{
    private readonly ApiDbContext _context;
    private readonly ILogger<WishlistRepository> _logger;

    public WishlistRepository(
        ApiDbContext context, 
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

    private async Task<WishlistElement?> GetWishlistElement(WishlistElement wishlistElement)
    {
        var result = await _context.Wishlists
            .Where(w => w.UserId == wishlistElement.UserId && w.ProductId == wishlistElement.ProductId)
            .ToListAsync();

        return result.FirstOrDefault();
    }

    public async Task<WishlistElement?> GetWishlistElement(int wishlistElementId)
    {
        var result = await _context.Wishlists
            .Where(w => w.Id == wishlistElementId)
            .ToListAsync();

        return result.FirstOrDefault();
    }

    private async Task<bool> IsWishlistElementExists(WishlistElement wishlistElement)
    {
        var result = await GetWishlistElement(wishlistElement);
        return result != null;
    }

    public async Task<bool> AddWishlist(WishlistElement wishlistElement)
    {
        if (await IsWishlistElementExists(wishlistElement)) return false;
        _context.Add(wishlistElement);
        return true;
    }

    public async Task<bool> DeleteWishlistElement(WishlistElement wishlistElement)
    {
        var foundElement = await GetWishlistElement(wishlistElement);
        if(foundElement == null) return false;
        _context.Remove(foundElement);
        return true;
    }

    public async Task<bool> DeleteWishlistElement(int wishlistElementId)
    {
        var foundElement = await GetWishlistElement(wishlistElementId);
        if (foundElement == null) return false;
        _context.Remove(foundElement);
        return true;
    }

    public async Task<int> SaveAllAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteWishlist(int userId)
    {
        var foundWishlist = await GetWishlist(userId);
        if (foundWishlist == null) return false;
        foreach (var foundWishlistElement in foundWishlist)
            _context.Remove(foundWishlistElement);
        return true;
    }
}
