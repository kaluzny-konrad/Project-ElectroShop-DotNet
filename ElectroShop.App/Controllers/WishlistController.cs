using ElectroShop.App.Business;
using ElectroShop.App.Models;
using ElectroShop.App.Services;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ElectroShop.App.Controllers;

public class WishlistController : Controller
{
    private readonly ILogger<WishlistController> _logger;
    private readonly IWishlistService _wishlistService;
    private readonly IProductService _productService;

    public WishlistController(
        ILogger<WishlistController> logger,
        IWishlistService wishlistService,
        IProductService productService)
    {
        _logger = logger;
        _wishlistService = wishlistService;
        _productService = productService;
    }

    public IActionResult Index()
    {
        var userId = 1;
        var userWishlist = _wishlistService.GetUserWishlist(userId);

        List<ProductInWishlistData> expandedUserWishlist = 
            userWishlist.Select(async wishlistElement => {
                var product = await _productService.GetProduct(wishlistElement.ProductId);
                if (product == null) return new ProductInWishlistData();

                var productImagePath = Url.Content($"~/images/product/{product.ProductId}-thumb.webp");
                var productPageUrl = $"/product/details/{product.ProductId}";

                var expandedWishlistElement = new ProductInWishlistData()
                {
                    ProductId = wishlistElement.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    ProductImagePath = productImagePath,
                    ProductPageUrl = productPageUrl,
                    WishlistElementId = wishlistElement.Id,
                    UserId = wishlistElement.UserId,
                    AddedDate = wishlistElement.AddedDate,
                };
                return expandedWishlistElement;
            }).Select(r => r.Result).Where(p => p.ProductId != 0).ToList();

        if (expandedUserWishlist.IsNullOrEmpty())
            return View("Empty");

        var model = new WishlistViewModel()
        {
            WishlistElements = expandedUserWishlist
        };

        return View("Index", model);
    }

    [HttpGet]
    public IActionResult Add(int id)
    {
        var userId = 1;
        var wishlistElement = new WishlistElement()
        {
            UserId = userId,
            ProductId = id,
            AddedDate = DateTime.UtcNow,
        };
        _wishlistService.AddWishlistElement(userId, wishlistElement);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int wishlistElementId)
    {
        var userId = 1;
        _wishlistService.DeleteWishlistElement(wishlistElementId, userId);
        return RedirectToAction("Index");
    }
}
