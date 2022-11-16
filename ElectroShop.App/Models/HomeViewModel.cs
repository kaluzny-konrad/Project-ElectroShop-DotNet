using ElectroShop.App.ModelEntities;

namespace ElectroShop.App.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ProductAtHome> Products { get; set; } = new List<ProductAtHome>();
    }
}
