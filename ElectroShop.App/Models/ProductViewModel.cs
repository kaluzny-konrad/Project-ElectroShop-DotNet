using ElectroShop.App.Business;
using ElectroShop.App.Controllers;

namespace ElectroShop.App.Models
{
    public class ProductViewModel
    {
        public BaseProductData Product { get; set; } = new();
    }
}
