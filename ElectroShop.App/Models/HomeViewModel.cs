using ElectroShop.App.Business;

namespace ElectroShop.App.Models;

public class HomeViewModel
{
    public IEnumerable<BaseProductData> Products { get; set; } 
        = new List<BaseProductData>();
}
