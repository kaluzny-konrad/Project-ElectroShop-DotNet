namespace ElectroShop.Shared.Domain
{
    public class ProductDescription
    {
        public int ProductDescriptionId { get; set; }
        public int ProductId { get; set; }
        public string ProductShortDescription { get; set; } = string.Empty;
        public string ProductLongDescription { get; set; } = string.Empty;
    }
}
