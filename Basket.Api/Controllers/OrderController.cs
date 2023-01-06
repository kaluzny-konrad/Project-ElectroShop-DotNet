using Basket.Api.Repositories;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IBasketItemsRepository _basketItemsRepository;

        public OrderController(IBasketItemsRepository basketItemsRepository)
        {
            _basketItemsRepository = basketItemsRepository;
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            return View();
        }
    }
}
