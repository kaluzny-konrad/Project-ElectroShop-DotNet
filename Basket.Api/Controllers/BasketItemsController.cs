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

        // GET api/<controller>/5
        [HttpGet("{userId}")]
        public IActionResult GetBasketItems(int userId)
        {
            if (userId <= 0) ModelState
                    .AddModelError("UserId", "UserId should be more than zero.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var basketItems = _basketItemsRepository.GetBasketItems(userId);
            return Ok(basketItems);
        }


        // POST api/<controller>
        [HttpPost]
        public IActionResult CreateBasketItem([FromBody] BasketItem basketItem)
        {
            if (basketItem.Amount <= 0) ModelState
                    .AddModelError("Amount", "Amount should be more than zero.");
            if (basketItem.UserId <= 0) ModelState
                    .AddModelError("UserId", "UserId should be more than zero.");
            if (basketItem.ProductId <= 0) ModelState
                    .AddModelError("ProductId", "ProductId should be more than zero.");

            var isBasketItemExists = _basketItemsRepository
                .IsBasketItemExists(basketItem);

            if (isBasketItemExists == true) ModelState
                .AddModelError("Uniqueness", "Product already exists in the user's cart.");
            
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedBasketItem = _basketItemsRepository.AddBasketItem(basketItem);
            return Ok(updatedBasketItem);
        }

        // PUT api/<controller>
        [HttpPut("{basketItemId}")]
        public IActionResult UpdateBasketItem([FromBody] BasketItem basketItem)
        {
            if (basketItem.BasketItemId <= 0) ModelState
                .AddModelError("BasketItemId", "BasketItemId should be more than zero.");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var foundBasketItem = _basketItemsRepository
                .GetBasketItem(basketItem.BasketItemId);
            if (foundBasketItem == null) 
                return NotFound();

            var updatedBasketItem = _basketItemsRepository
                .UpdateBasketItem(basketItem);
            return Ok(updatedBasketItem);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{basketItemId}")]
        public IActionResult DeleteBasketItem(int basketItemId)
        {
            if (basketItemId <= 0) ModelState
                .AddModelError("BasketItemId", "BasketItemId should be more than zero.");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var basketItemToDelete = _basketItemsRepository
                .GetBasketItem(basketItemId);
            if (basketItemToDelete == null) return NotFound();

            _basketItemsRepository.DeleteBasketItem(basketItemId);

            return Ok(basketItemToDelete);
        }
    }
}
