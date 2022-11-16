using ElectroShop.App.Entities;

namespace ElectroShop.App.Services
{
    public interface IProductDescriptionService
    {
        IEnumerable<ProductDescription> GetProductDescriptions();
        ProductDescription? GetProductDescription(int productId);
        string? GetShortDescription(int productId);
        string? GetLongProductDescription(int productId);
    }

    public class ProductDescriptionService : IProductDescriptionService
    {
        public string? GetLongProductDescription(int productId)
        {
            return GetProductDescription(productId)?.ProductLongDescription;
        }

        public string? GetShortDescription(int productId)
        {
            return GetProductDescription(productId)?.ProductShortDescription;
        }

        public ProductDescription? GetProductDescription(int productId)
        {
            return GetProductDescriptions().FirstOrDefault(pd => pd.ProductId == productId);
        }

        public IEnumerable<ProductDescription> GetProductDescriptions()
        {
            return new List<ProductDescription> 
            { 
                new ProductDescription 
                {
                    ProductDescriptionId = 1,
                    ProductId = 139108825,
                    ProductShortDescription = 
@"Smartfon Motorola z ekranem 6,7 cala, wyświetlacz OLED. Aparat 108 Mpix, pamięć 8 GB RAM, bateria 4000mAh. Obsługuje sieć: 5G",
                    ProductLongDescription = 
@"Ultrasmukły i lekki, z kinowym ekranem 6,28"", oferowany w najnowszych modnych kolorach Pantone. 
System aparatów 64 Mpx z OIS 
Rób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, 
jednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.",
                },
                new ProductDescription
                {
                    ProductDescriptionId = 2,
                    ProductId = 139108829,
                    ProductShortDescription =
@"Smartfon Motorola z ekranem 6,7 cala, wyświetlacz OLED. Aparat 108 Mpix, pamięć 8 GB RAM, bateria 4000mAh. Obsługuje sieć: 5G",
                    ProductLongDescription =
@"Ultrasmukły i lekki, z kinowym ekranem 6,28"", oferowany w najnowszych modnych kolorach Pantone. 
System aparatów 64 Mpx z OIS 
Rób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, 
jednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.",
                },
                new ProductDescription
                {
                    ProductDescriptionId = 3,
                    ProductId = 139120876,
                    ProductShortDescription =
@"Smartfon Motorola z ekranem 6,7 cala, wyświetlacz OLED. Aparat 108 Mpix, pamięć 8 GB RAM, bateria 4000mAh. Obsługuje sieć: 5G",
                    ProductLongDescription =
@"Ultrasmukły i lekki, z kinowym ekranem 6,28"", oferowany w najnowszych modnych kolorach Pantone. 
System aparatów 64 Mpx z OIS 
Rób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, 
jednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.",
                },
                new ProductDescription
                {
                    ProductDescriptionId = 4,
                    ProductId = 113375066,
                    ProductShortDescription =
@"Smartfon Motorola z ekranem 6,7 cala, wyświetlacz OLED. Aparat 108 Mpix, pamięć 8 GB RAM, bateria 4000mAh. Obsługuje sieć: 5G",
                    ProductLongDescription =
@"Ultrasmukły i lekki, z kinowym ekranem 6,28"", oferowany w najnowszych modnych kolorach Pantone. 
System aparatów 64 Mpx z OIS 
Rób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, 
jednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.",
                },
                new ProductDescription
                {
                    ProductDescriptionId = 5,
                    ProductId = 138536499,
                    ProductShortDescription =
@"Smartfon Apple z ekranem 6,1 cala, wyświetlacz Super Retina XDR. Aparat 48 Mpix, pamięć 4 GB RAM. Obsługuje sieć: 5G",
                    ProductLongDescription =
@"Ultrasmukły i lekki, z kinowym ekranem 6,28"", oferowany w najnowszych modnych kolorach Pantone. 
System aparatów 64 Mpx z OIS 
Rób krystalicznie wyraźne zdjęcia w wysokiej rozdzielczości w każdym oświetleniu, 
jednocześnie eliminując rozmycia na skutek przypadkowego poruszenia aparatem.",
                },
            };
        }
    }
}
