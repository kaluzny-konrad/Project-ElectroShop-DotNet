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
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductDescriptionService _productDescriptionService;

        public HomeController
        (
            ILogger<HomeController> logger, 
            IProductService productService, 
            IManufacturerService manufacturerService, 
            IProductDescriptionService productDescriptionService
        )
        {
            _logger = logger;
            _productService = productService;
            _manufacturerService = manufacturerService;
            _productDescriptionService = productDescriptionService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();

            var categoryDataProducts = new List<CategoryDataProduct>();

            foreach (var product in products)
            {
                var manufacturer = await _manufacturerService.GetManufacturer(product.ManufacturerId);
                var shortDescription = await _productDescriptionService.GetShortDescription(product.ProductId);
                var productImagePath = $"images/product/{product.ProductId}.webp";

                var categoryDataProduct = new CategoryDataProduct
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Manufacturer = manufacturer,
                    ProductShortDescription = shortDescription,
                    ProductImagePath = productImagePath,
                };

                categoryDataProducts.Add(categoryDataProduct);
            }

            var model = new HomeViewModel { Products = categoryDataProducts };
            return View(model);
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
}