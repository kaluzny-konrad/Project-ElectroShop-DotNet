using ElectroShop.gRPC;
using ElectroShop.Shared.Domain;
using Google.Protobuf.WellKnownTypes;
using static ElectroShop.gRPC.WishlistService;

namespace ElectroShop.App.Services;

public interface IWishlistService
{
    public List<WishlistElement> GetUserWishlist(int userId);
    public bool AddWishlistElement(int userId, WishlistElement element);
    public bool DeleteWishlistElement(int wishlistElementId, int userId);
}

public class WishlistService : IWishlistService
{
    private readonly ILogger<WishlistService> _logger;
    private readonly WishlistServiceClient _client;

    public WishlistService(ILogger<WishlistService> logger,
        WishlistServiceClient client)
    {
        _logger = logger;
        _client = client;
    }

    public List<WishlistElement> GetUserWishlist(int userId)
    {
        var userInfo = new UserInfo()
        {
            UserId = userId,
        };
        var userWishlist = new List<WishlistElement>();

        var userWishlistGrpc = _client.GetWishlistByUserId(userInfo);
        if(userWishlistGrpc == null) return userWishlist;

        foreach (var wishlistElementGrpc in userWishlistGrpc.WishlistElements) 
        { 
            if(wishlistElementGrpc != null)
            {
                var wishlistElement = new WishlistElement
                {
                    Id = wishlistElementGrpc.WishlistElementId,
                    UserId = wishlistElementGrpc.UserId,
                    ProductId = wishlistElementGrpc.ProductId,
                    AddedDate = wishlistElementGrpc.AddedDate.ToDateTime(),
                };
                userWishlist.Add(wishlistElement);
            }
        }

        return userWishlist;
    }

    public bool AddWishlistElement(int userId, WishlistElement element)
    {
        var wishlistData = new WishlistElementGrpc()
        {
            UserId = userId,
            ProductId = element.ProductId,
            AddedDate = element.AddedDate.ToTimestamp()
        };

        var wishlistsDataPacket = new WishlistPacket()
        {
            Successful = WishlistStatus.Success
        };

        wishlistsDataPacket.WishlistElements.Add(wishlistData);

        var statusMessage = _client.AddWishlist(wishlistsDataPacket);
        if (statusMessage == null) return false;
        if (statusMessage.Status == WishlistStatus.Success) return true;
        return false;
    }

    public bool DeleteWishlistElement(int wishlistElementId, int userId)
    {
        var wishlistData = new WishlistElementToDeleteById()
        {
            WishlistElementId = wishlistElementId,
            UserId = userId,
        };

        var statusMessage = _client.DeteleWishlistElementById(wishlistData);
        if (statusMessage == null) return false;
        if (statusMessage.Status == WishlistStatus.Success) return true;
        return false;
    }
}
