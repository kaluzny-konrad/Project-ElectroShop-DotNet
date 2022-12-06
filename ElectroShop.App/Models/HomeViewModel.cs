using ElectroShop.App.Business;
using ElectroShop.App.Controllers;

namespace ElectroShop.App.Models
{
    public class HomeViewModel
    {
        public IEnumerable<CategoryDataProduct> Products { get; set; } = new List<CategoryDataProduct>();
    }
}
