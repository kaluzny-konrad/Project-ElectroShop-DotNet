using ElectroShop.App.Entities;
using ElectroShop.App.ModelEntities;
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

        public IActionResult Index()
        {
            var products = _productService.GetProducts();

            var productsAtHome = products.Select(p =>
            {
                var manufacturer = _manufacturerService.GetManufacturer(p.ManufacturerId);
                if (manufacturer == null)
                {
                    manufacturer = new Manufacturer();
                    _logger.LogError("Manufacturer is null at product of id: {ProductId}", p.ProductId);
                }
                    

                var shortDescription = _productDescriptionService.GetShortDescription(p.ProductId);
                if (shortDescription == null)
                {
                    shortDescription = string.Empty;
                    _logger.LogError("ShortDescription is null at product of id: {ProductId}", p.ProductId);
                }

                var productImagePath = $"images/product/{p.ProductId}.webp";

                return new ProductAtHome
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Manufacturer = manufacturer,
                    ProductShortDescription = shortDescription,
                    ProductImagePath = productImagePath,
                };
            });

            var model = new HomeViewModel { Products = productsAtHome };
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