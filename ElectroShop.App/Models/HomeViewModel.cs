using ElectroShop.App.Business;

namespace ElectroShop.App.Models
{
    public class HomeViewModel
    {
        public IEnumerable<CategoryDataProduct> Products { get; set; } = new List<CategoryDataProduct>();
    }
}
