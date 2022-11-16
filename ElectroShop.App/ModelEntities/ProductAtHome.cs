using ElectroShop.App.Entities;

namespace ElectroShop.App.ModelEntities
{
    public class ProductAtHome
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public float Price { get; set; }
        public Manufacturer Manufacturer { get; set; } = new Manufacturer();
        public string ProductShortDescription { get; set; } = string.Empty;
        public string ProductImagePath { get; set; } = string.Empty;
    }
}
