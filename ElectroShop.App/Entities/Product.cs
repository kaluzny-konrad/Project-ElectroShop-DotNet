namespace ElectroShop.App.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public float Price { get; set; }
        public int ManufacturerId { get; set; }
    }
}
