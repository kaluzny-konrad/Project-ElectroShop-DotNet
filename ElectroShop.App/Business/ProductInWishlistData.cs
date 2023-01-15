namespace ElectroShop.App.Business
{
    public class ProductInWishlistData
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ProductImagePath { get; set; } = string.Empty;
        public string ProductPageUrl { get; set; } = string.Empty;
        public int WishlistElementId { get; set; }
        public int UserId { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
    }
}
