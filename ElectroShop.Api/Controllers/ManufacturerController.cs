using ElectroShop.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElectroShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerController(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult GetManufacturers()
        {
            return Ok(_manufacturerRepository.GetManufacturers());
        }

        // GET api/<controller>/5
        [HttpGet("{manufacturerId}")]
        public IActionResult GetManufacturer(int manufacturerId)
        {
            return Ok(_manufacturerRepository.GetManufacturer(manufacturerId));
        }
    }
}
