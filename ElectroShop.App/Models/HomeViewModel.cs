using ElectroShop.App.Business;
using ElectroShop.App.Controllers;

namespace ElectroShop.App.Models
{
    public class HomeViewModel
    {
        public IEnumerable<BaseProductData> Products { get; set; } 
            = new List<BaseProductData>();
    }
}
