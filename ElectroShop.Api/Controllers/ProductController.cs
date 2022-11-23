using ElectroShop.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElectroShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_productRepository.GetProducts());
        }

        // GET api/<controller>/5
        [HttpGet("{productId}")]
        public IActionResult GetProduct(int productId)
        {
            return Ok(_productRepository.GetProduct(productId));
        }
    }
}
