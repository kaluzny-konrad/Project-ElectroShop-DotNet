using Microsoft.AspNetCore.Mvc;

namespace ElectroShop.App.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add(int id)
        {
            return RedirectToAction("Index");
        }
    }
}
