using ElectroShop.App.Business;

namespace ElectroShop.App.Models;

public class BasketViewModel
{
    public List<ProductInBasketData> BasketItems { get; set; } = new();
}
