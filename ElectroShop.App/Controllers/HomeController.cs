using ElectroShop.App.Business;
using ElectroShop.App.Models;
using ElectroShop.App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElectroShop.App.Controllers
{
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
            var categoryDataProducts = await GetCategoryDataProducts();

            var model = new HomeViewModel
            {
                Products = categoryDataProducts,
            };

            return View(model);
        }

        private async Task<List<CategoryDataProduct>> GetCategoryDataProducts()
        {
            var products = await _productService.GetProducts();

            var categoryDataProducts = new List<CategoryDataProduct>();

            foreach (var product in products)
            {
                var productImagePath = $"images/product/{product.ProductId}-thumb.webp";
                var productPageUrl = $"/product/{product.ProductId}";

                var categoryDataProduct = new CategoryDataProduct
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

    public enum CategoryViewType
    {
        List = 0,
        Box = 1,
    }
}