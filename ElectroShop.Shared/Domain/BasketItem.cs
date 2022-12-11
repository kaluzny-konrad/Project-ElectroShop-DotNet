namespace ElectroShop.Shared.Domain
{
    public class BasketItem
    {
        public int BasketItemId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Amount { get; set; }
    }
}
