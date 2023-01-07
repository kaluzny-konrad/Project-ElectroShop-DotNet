using ElectroShop.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElectroShop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufacturerController : Controller
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly ILogger<ManufacturerController> _logger;

    public ManufacturerController(
        IManufacturerRepository manufacturerRepository, 
        ILogger<ManufacturerController> logger)
    {
        _manufacturerRepository = manufacturerRepository;
        _logger = logger;
    }

    // GET: api/<controller>
    [HttpGet]
    public IActionResult GetManufacturers()
    {
        try
        {
            var result = _manufacturerRepository.GetManufacturers();
            return Ok(result);
        }
        catch (Exception ex)
        {
            LogException(ex);
            return StatusCode(500);
        }
    }

    // GET api/<controller>/5
    [HttpGet("{manufacturerId}")]
    public IActionResult GetManufacturer(int manufacturerId)
    {
        if (manufacturerId <= 0)
            ModelState.AddModelError(
                "manufacturerId", "manufacturerId should be more than zero.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = _manufacturerRepository.GetManufacturer(manufacturerId);
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
            " on the Manufacturer Repository: {ex.Message}",
            ex.Message);
    }
}
