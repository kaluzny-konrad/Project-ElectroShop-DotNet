namespace ElectroShop.App.Business
{
    public class ProductInBasketData
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ProductImagePath { get; set; } = string.Empty;
        public string ProductPageUrl { get; set; } = string.Empty;
        public int Amount { get; set; }
        public decimal FullPrice { get; set; }
        public int BasketItemId { get; set; }
    }
}
