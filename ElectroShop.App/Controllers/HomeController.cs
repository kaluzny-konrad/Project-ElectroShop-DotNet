using ElectroShop.App.Business;
using ElectroShop.App.Models;
using ElectroShop.App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElectroShop.App.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IProductService _productService;

    public HomeController
    (
        ILogger<HomeController> logger, 
        IProductService productService
    )
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await GetProductsBaseData();

        var model = new HomeViewModel
        {
            Products = products,
        };

        return View(model);
    }

    private async Task<List<BaseProductData>> GetProductsBaseData()
    {
        var top = 3;
        var products = await _productService.GetProducts(top);

        var categoryDataProducts = new List<BaseProductData>();

        foreach (var product in products)
        {
            var productImagePath = Url.Content($"~/images/product/{product.ProductId}-thumb.webp");
            var productPageUrl = $"/product/details/{product.ProductId}";

            var categoryDataProduct = new BaseProductData
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                ProductImagePath = productImagePath,
                ProductPageUrl = productPageUrl,
            };

            categoryDataProducts.Add(categoryDataProduct);
        }

        return categoryDataProducts;
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}