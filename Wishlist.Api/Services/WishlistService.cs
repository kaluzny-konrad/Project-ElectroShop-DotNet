using ElectroShop.gRPC;
using ElectroShop.Shared.Domain;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Wishlist.Api.Migrations;
using Wishlist.Api.Repositories;
using static ElectroShop.gRPC.WishlistService;

namespace Wishlist.Api.Services;

public class WishlistService : WishlistServiceBase
{
    private readonly IWishlistRepository _repository;
    private readonly ILogger<WishlistService> _logger;

    public WishlistService(
        IWishlistRepository repository,
        ILogger<WishlistService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public override async Task<UserWishlist> GetWishlistByUserId(
        UserInfo request, 
        ServerCallContext context)
    {
        var userId = request.UserId;
        var wishlist = await _repository.GetWishlist(userId);
        var userWishlist = new UserWishlist();
        foreach (var wishlistElement in wishlist)
        {
            var addedDate = wishlistElement.AddedDate.ToUniversalTime();
            var wishlistMessage = new WishlistElementGrpc()
            {
                UserId = wishlistElement.UserId,
                ProductId = wishlistElement.ProductId,
                AddedDate = addedDate.ToTimestamp()
            };
            userWishlist.WishlistElements.Add(wishlistMessage);
        }
        
        return userWishlist;
    }

    public override async Task<StatusMessage> AddWishlist(
        WishlistPacket request,
        ServerCallContext context)
    {
        if (request.Successful != WishlistStatus.Success)
            return HandleSaveFailure();

        int wishlistElementsToSave = 0;
        foreach (var wishlistElementRequest in request.WishlistElements)
            if (await SaveWishlistElement(wishlistElementRequest))
                wishlistElementsToSave++;

        if (wishlistElementsToSave > 0)
        {
            var savedCount = await _repository.SaveAllAsync();
            if (savedCount > 0)
                return HandleSaveSuccess(savedCount);
        }
        else return HandleNoChanges();

        return HandleSaveFailure();
    }

    private async Task<bool> SaveWishlistElement(WishlistElementGrpc? wishlistElementRequest)
    {
        if (wishlistElementRequest == null) return false;

        var wishlistElement = new WishlistElement();
        try
        {
            wishlistElement.UserId = wishlistElementRequest.UserId;
            wishlistElement.ProductId = wishlistElementRequest.ProductId;
            var addedTime = wishlistElementRequest.AddedDate.ToDateTime();
            wishlistElement.AddedDate = addedTime;
        }
        catch(Exception ex)
        {
            _logger.LogError("Failed to get wishlist element: {ex.Message}", ex.Message);
            return false;
        }

        _logger.LogInformation("Adding wishlistElement: {wishlistElement}", wishlistElement);
        if(await _repository.AddWishlist(wishlistElement))
        {
            _logger.LogInformation("WishlistElement added successfully to Database");
            return true;
        }
        else
        {
            _logger.LogInformation("WishlistElement already existed in Database");
            return false;
        }
    }

    private StatusMessage HandleSaveSuccess(int savedCount)
    {
        _logger.LogInformation("Successfully changed Wishlists");
        return new StatusMessage()
        {
            StatusDetails = $"Successfully changed {savedCount} wishlists",
            Status = WishlistStatus.Success,
        };
    }

    private StatusMessage HandleSaveFailure()
    {
        _logger.LogInformation("Failed to change Wishlists");
        return new StatusMessage()
        {
            StatusDetails = "Failed to change Wishlists",
            Status = WishlistStatus.Failure
        };
    }

    private StatusMessage HandleNoChanges()
    {
        _logger.LogInformation("No wishlist has been changed");
        return new StatusMessage()
        {
            StatusDetails = "No wishlist has been changed",
            Status = WishlistStatus.Success
        };
    }

    public async override Task AddWishlistStream(
        IAsyncStreamReader<WishlistElementGrpc> requestStream,
        IServerStreamWriter<ErrorMessage> responseStream,
        ServerCallContext context)
    {
        while (await requestStream.MoveNext())
        {
            var wishlistElementRequest = requestStream.Current;
            await SaveWishlistElement(wishlistElementRequest);
            if (await _repository.SaveAllAsync() == 0)
                await HandleSaveFailureStream(responseStream);
        }
    }

    private static async Task HandleSaveFailureStream(
        IServerStreamWriter<ErrorMessage> responseStream)
    {
        await responseStream.WriteAsync(new ErrorMessage()
        {
            Message = $"Failed to store in Database"
        });
    }

    public override async Task<StatusMessage> DeleteWishlistElements(
        WishlistElementsToDelete request, 
        ServerCallContext context)
    {
        var wishlistElementsToDelete = request.WishlistElements;
        int wishlistElementsToSave = 0;
        foreach (var wishlistElementToDelete in wishlistElementsToDelete)
        {
            var wishlistElement = new WishlistElement
            {
                UserId = wishlistElementToDelete.UserId,
                ProductId = wishlistElementToDelete.ProductId
            };

            if (await _repository.DeleteWishlistElement(wishlistElement))
                wishlistElementsToSave++;
        }

        if (wishlistElementsToSave > 0)
        {
            var savedCount = await _repository.SaveAllAsync();
            if (savedCount > 0)
                return HandleSaveSuccess(savedCount);
        }
        else return HandleNoChanges();

        return HandleSaveFailure();
    }

    public override async Task<StatusMessage> DeleteWishlist(
        WishlistsToDelete request, 
        ServerCallContext context)
    {
        var UserWishlistToDelete = request.Users;
        int changesToSave = 0;
        foreach (var userInfo in UserWishlistToDelete)
        {
            var userId = userInfo.UserId;

            if (await _repository.DeleteWishlist(userId))
                changesToSave++;
        }

        if (changesToSave > 0)
        {
            var savedChanges = await _repository.SaveAllAsync();
            if (savedChanges > 0)
                return HandleSaveSuccess(savedChanges);
        }
        else return HandleNoChanges();

        return HandleSaveFailure();
    }
}
