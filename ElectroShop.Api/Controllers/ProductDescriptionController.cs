using ElectroShop.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElectroShop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductDescriptionController : Controller
{
    private readonly IProductDescriptionRepository _productDescriptionRepository;
    private readonly ILogger<ProductDescriptionController> _logger;

    public ProductDescriptionController(
        IProductDescriptionRepository productDescriptionRepository, 
        ILogger<ProductDescriptionController> logger)
    {
        _productDescriptionRepository = productDescriptionRepository;
        _logger = logger;
    }

    // GET: api/<controller>
    [HttpGet]
    public IActionResult GetProductDescriptions()
    {
        try
        {
            var result = _productDescriptionRepository.GetProductDescriptions();
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
    public IActionResult GetProductDescription(int productId)
    {
        if (productId <= 0)
            ModelState.AddModelError("productId",
                "productId should be more than zero.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = _productDescriptionRepository.GetProductDescription(productId);
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
            " on the ProductDescription Repository: {ex.Message}",
            ex.Message);
    }
}
