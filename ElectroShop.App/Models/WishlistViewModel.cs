using ElectroShop.App.Business;

namespace ElectroShop.App.Models;

public class WishlistViewModel
{
    public List<ProductInWishlistData> WishlistElements { get; set;} = new();
}
