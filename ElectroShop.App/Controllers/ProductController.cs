using ElectroShop.App.Business;
using ElectroShop.App.Models;
using ElectroShop.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElectroShop.App.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;

    private readonly IProductService _productService;

    public ProductController
    (
        ILogger<ProductController> logger, 
        IProductService productService
    )
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await GetBaseProductData(id);

        var model = new ProductViewModel
        {
            Product = product,
        };

        return View("Details", model);
    }

    private async Task<BaseProductData> GetBaseProductData(int productId)
    {
        var product = await _productService.GetProduct(productId);
        var productImagePath = Url.Content($"~/images/product/{product.ProductId}-thumb.webp");
        var productPageUrl = $"/product/details/{product.ProductId}";

        var productPageData = new BaseProductData
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            Price = product.Price,
            ProductImagePath = productImagePath,
            ProductPageUrl = productPageUrl,
        };

        return productPageData;
    }
}
