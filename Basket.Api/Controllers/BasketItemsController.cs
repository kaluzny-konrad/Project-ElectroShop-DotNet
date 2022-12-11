using Basket.Api.Repositories;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController : Controller
    {
        private readonly IBasketItemsRepository _basketItemsRepository;

        public BasketItemsController(IBasketItemsRepository basketItemsRepository)
        {
            _basketItemsRepository = basketItemsRepository;
        }

        // GET api/<controller>
        public IActionResult GetBasketItems()
        {
            var basketItems = _basketItemsRepository.GetBasketItems();
            if (basketItems.IsNullOrEmpty()) return NotFound();

            return Ok(basketItems);
        }

        // GET api/<controller>/5
        [HttpGet("{userId}")]
        public IActionResult GetBasketItems(int userId)
        {
            if (userId == 0) return BadRequest();

            var basketItems = _basketItemsRepository.GetBasketItems(userId);
            if (basketItems.IsNullOrEmpty()) return NotFound();

            return Ok(basketItems);
        }


        // POST api/<controller>
        [HttpPost]
        public IActionResult CreateBasketItem([FromBody] BasketItem basketItem)
        {
            if (basketItem == null) return BadRequest();

            if (basketItem.Amount == 0) ModelState
                    .AddModelError("Amount", "Amount shouldn't be zero.");
            if (basketItem.UserId == 0) ModelState
                    .AddModelError("UserId", "UserId shouldn't be zero.");
            if (basketItem.ProductId == 0) ModelState
                    .AddModelError("ProductId", "ProductId shouldn't be zero.");

            var foundBasketItem = _basketItemsRepository
                .GetBasketItem(basketItem.UserId, basketItem.ProductId);

            if (foundBasketItem != null) ModelState
                .AddModelError("UserId ProductId", "Product already exists in the user's cart.");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedBasketItem = _basketItemsRepository.AddBasketItem(basketItem);
            return Ok(updatedBasketItem);
        }


        // POST api/<controller>/5
        [HttpPost("{basketItemId}")]
        public IActionResult AddAmountToBasketItem(int basketItemId, [FromBody] int amount)
        {
            if (amount == 0) return BadRequest();

            var foundBasketItem = _basketItemsRepository
                .GetBasketItem(basketItemId);

            if (foundBasketItem == null) return NotFound();

            var updatedBasketItem = _basketItemsRepository
                .AddAmountToBasketItem(basketItemId, amount);
            return Ok(updatedBasketItem);
        }

        // PUT api/<controller>/5
        [HttpPut("{basketItemId}")]
        public IActionResult UpdateAmountOfBasketItem(int basketItemId, [FromBody] int amount)
        {
            if (amount == 0) return BadRequest();

            var foundBasketItem = _basketItemsRepository
                .GetBasketItem(basketItemId);

            if (foundBasketItem == null) return NotFound();

            var updatedBasketItem = _basketItemsRepository
                .UpdateBasketItem(basketItemId, amount);
            return Ok(updatedBasketItem);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{basketItemId}")]
        public IActionResult DeleteBasketItem(int basketItemId)
        {
            if (basketItemId == 0) return BadRequest();

            var basketItemToDelete = _basketItemsRepository
                .GetBasketItem(basketItemId);
            if (basketItemToDelete == null) return NotFound();

            _basketItemsRepository.DeleteBasketItem(basketItemId);

            return Ok(basketItemToDelete);
        }
    }
}
