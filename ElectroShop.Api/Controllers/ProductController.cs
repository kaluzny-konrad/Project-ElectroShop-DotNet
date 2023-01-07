using ElectroShop.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElectroShop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductController> _logger;

    public ProductController(
        IProductRepository productRepository, 
        ILogger<ProductController> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    // GET: api/<controller>
    [HttpGet]
    public IActionResult GetProducts()
    {
        try
        {
            var result = _productRepository.GetProducts();
            return Ok(result);
        }
        catch (Exception ex)
        {
            LogException(ex);
            return StatusCode(500);
        }
    }

    // GET api/<controller>/5
    [HttpGet("{productId}")]
    public IActionResult GetProduct(int productId)
    {
        if (productId <= 0)
            ModelState.AddModelError("productId", 
                "productId should be more than zero.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = _productRepository.GetProduct(productId);
            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            LogException(ex);
            return StatusCode(500);
        }
    }

    private void LogException(Exception ex)
    {
        _logger.LogError("An error occurred while working" +
            " on the Product Repository: {ex.Message}", 
            ex.Message);
    }
}
