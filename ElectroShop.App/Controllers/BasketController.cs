using ElectroShop.App.Business;
using ElectroShop.App.Models;
using ElectroShop.App.Services;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ElectroShop.App.Controllers;

public class BasketController : Controller
{
    private readonly ILogger<BasketController> _logger;
    private readonly IBasketService _basketService;
    private readonly IProductService _productService;

    public BasketController(
        ILogger<BasketController> logger,
        IBasketService basketService, 
        IProductService productService)
    {
        _logger = logger;
        _basketService = basketService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var basketItems = await _basketService.GetBasketItems(1);

        if (basketItems.IsNullOrEmpty())
            return View("Empty");
        
        List<ProductInBasketData> expandedBasketItems = 
            basketItems.Select(async basketItem =>
                {
                    var product = await _productService.GetProduct(basketItem.ProductId);
                    if (product == null) return new ProductInBasketData();

                    var productImagePath = Url.Content($"~/images/product/{product.ProductId}-thumb.webp");
                    var productPageUrl = $"/product/details/{product.ProductId}";

                    var expandedBasketItem = new ProductInBasketData()
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        ProductImagePath = productImagePath,
                        ProductPageUrl = productPageUrl,
                        Amount = basketItem.Amount,
                        FullPrice = basketItem.Amount * product.Price,
                        BasketItemId = basketItem.BasketItemId,
                    };
                    return expandedBasketItem;
                }).Select(r => r.Result).Where(p => p.ProductId != 0).ToList();

        if (expandedBasketItems.IsNullOrEmpty())
            return View("Empty");

        var model = new BasketViewModel() { BasketItems = expandedBasketItems };
        return View("Index", model);
    }

    [HttpGet]
    public async Task<IActionResult> Add(int id)
    {
        var basketItems = await _basketService.GetBasketItems(1);
        var basketItem = basketItems.FirstOrDefault(i => i.ProductId == id);
        if(basketItem != null)
        {
            basketItem.Amount++;
            await _basketService.UpdateBasketItem(basketItem);
        }
        else
        {
            basketItem = new BasketItem() 
                { UserId = 1, ProductId = id, Amount = 1 };
            await _basketService.CreateBasketItem(basketItem);
        }
        
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int basketItemId)
    {
        await _basketService.DeleteBasketItem(basketItemId);
        return RedirectToAction("Index");
    }
}
