namespace ElectroShop.Shared.Domain
{
    public class UserBasket
    {
        public int UserId { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new();
    }
}
