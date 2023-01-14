using Basket.Api.Repositories;
using ElectroShop.Shared.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketItemsController : Controller
{
    private readonly IBasketItemsRepository _basketItemsRepository;
    private readonly ILogger<BasketItemsController> _logger;

    public BasketItemsController(
        IBasketItemsRepository basketItemsRepository, 
        ILogger<BasketItemsController> logger)
    {
        _basketItemsRepository = basketItemsRepository;
        _logger = logger;
    }

    // GET api/<controller>/5
    [HttpGet("{userId}")]
    public IActionResult GetBasketItems(int userId)
    {
        if (userId <= 0) 
            ModelState.AddModelError("userId",
                "userId should be more than zero.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var basketItems = _basketItemsRepository.GetBasketItems(userId);
            return Ok(basketItems);
        }
        catch (Exception ex)
        {
            LogException(ex);
            return StatusCode(500);
        }
    }

    // POST api/<controller>
    [HttpPost]
    public IActionResult CreateBasketItem([FromBody] BasketItem basketItem)
    {
        if (basketItem.Amount <= 0) ModelState.AddModelError("Amount", 
            "Amount should be more than zero.");
        if (basketItem.UserId <= 0) ModelState.AddModelError("UserId", 
            "UserId should be more than zero.");
        if (basketItem.ProductId <= 0) ModelState.AddModelError("ProductId", 
            "ProductId should be more than zero.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var isBasketItemExists = _basketItemsRepository
                .IsBasketItemExists(basketItem);

            if (isBasketItemExists == true) ModelState.AddModelError("Uniqueness", 
                "Product already exists in the user's cart.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedBasketItem = _basketItemsRepository
                .AddBasketItem(basketItem);
            if (updatedBasketItem == null) ThrowNoChangesError();

            return Ok(updatedBasketItem);
        }
        catch (Exception ex)
        {
            LogException(ex);
            return StatusCode(500);
        }
    }

    // PUT api/<controller>
    [HttpPut("{basketItemId}")]
    public IActionResult UpdateBasketItem([FromBody] BasketItem basketItem)
    {
        if (basketItem.BasketItemId <= 0) ModelState.AddModelError("basketItemId",
            "basketItemId should be more than zero.");
        if (basketItem.Amount <= 0) ModelState.AddModelError("Amount",
            "Amount should be more than zero.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var foundBasketItem = _basketItemsRepository
                .GetBasketItem(basketItem.BasketItemId);
            if (foundBasketItem == null) return NotFound();

            var updatedBasketItem = _basketItemsRepository.UpdateBasketItem(basketItem);
            if (updatedBasketItem == null) ThrowNoChangesError();

            return Ok(updatedBasketItem);
        }
        catch (Exception ex)
        {
            LogException(ex);
            return StatusCode(500);
        }
    }

    // DELETE api/<controller>/5
    [HttpDelete("{basketItemId}")]
    public IActionResult DeleteBasketItem(int basketItemId)
    {
        if (basketItemId <= 0) ModelState.AddModelError("basketItemId",
            "basketItemId should be more than zero.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var basketItemToDelete = _basketItemsRepository
                .GetBasketItem(basketItemId);
            if (basketItemToDelete == null) return NotFound();

            if(_basketItemsRepository.DeleteBasketItem(basketItemId))
                ThrowNoChangesError();

            return Ok(basketItemToDelete);
        }
        catch (Exception ex)
        {
            LogException(ex);
            return StatusCode(500);
        }
    }

    private static void ThrowNoChangesError()
        => throw new Exception("Changes have not been saved to the database.");

    private void LogException(Exception ex)
    {
        _logger.LogError("An error occurred while working" +
            " on the BasketItems Repository: {ex.Message}", 
            ex.Message);
    }
}
