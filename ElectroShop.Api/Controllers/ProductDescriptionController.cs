using ElectroShop.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElectroShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDescriptionController : Controller
    {
        private readonly IProductDescriptionRepository _productDescriptionRepository;

        public ProductDescriptionController(IProductDescriptionRepository productDescriptionRepository)
        {
            _productDescriptionRepository = productDescriptionRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult GetProductDescriptions()
        {
            return Ok(_productDescriptionRepository.GetProductDescriptions());
        }

        // GET api/<controller>/5
        [HttpGet("{productId}")]
        public IActionResult GetProductDescription(int productId)
        {
            return Ok(_productDescriptionRepository.GetProductDescription(productId));
        }
    }
}
