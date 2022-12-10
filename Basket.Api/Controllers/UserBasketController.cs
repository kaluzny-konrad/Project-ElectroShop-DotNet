using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    public class UserBasketController : Controller
    {
        private readonly IUserBasketRepository _userBasketRepository;

        public UserBasketController(IUserBasketRepository userBasketRepository)
        {
            _userBasketRepository = userBasketRepository;
        }

        // GET api/<controller>/5
        [HttpGet("{userId}")]
        public IActionResult GetUserBasket(int userId)
        {
            return Ok(_userBasketRepository.GetUserBasket(userId));
        }
    }
}
