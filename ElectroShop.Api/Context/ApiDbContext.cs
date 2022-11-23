using ElectroShop.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace ElectroShop.Api.Context
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) 
            : base(options) {}

        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDescription> ProductDescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Manufacturer>()
                .HasData(new Manufacturer
                {
                    ManufacturerId = 1,
                    ManufacturerName = "Motorola"
                });

            modelBuilder.Entity<Manufacturer>()
                .HasData(new Manufacturer
                {
                    ManufacturerId = 2,
                    ManufacturerName = "Apple"
                });

            modelBuilder.Entity<ProductDescription>()
                .HasData(new ProductDescription
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
                });

            modelBuilder.Entity<ProductDescription>()
                .HasData(new ProductDescription
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
                });

            modelBuilder.Entity<ProductDescription>()
                .HasData(new ProductDescription
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
                });

            modelBuilder.Entity<ProductDescription>()
                .HasData(new ProductDescription
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
                });

            modelBuilder.Entity<ProductDescription>()
                .HasData(new ProductDescription
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
                });

            modelBuilder.Entity<Product>()
                .HasData(new Product
                {
                    ProductId = 139108825,
                    ProductName = "Motorola Edge 30 Neo 8/128GB Czarny",
                    Price = 1749M,
                    ManufacturerId = 1,
                });

            modelBuilder.Entity<Product>()
                .HasData(new Product
                {
                    ProductId = 139108829,
                    ProductName = "Motorola Edge 30 Neo 8/128GB Fioletowy",
                    Price = 1749M,
                    ManufacturerId = 1,
                });

            modelBuilder.Entity<Product>()
                .HasData(new Product
                {
                    ProductId = 139120876,
                    ProductName = "Motorola Edge 30 Neo 8/128GB Srebrny",
                    Price = 1749M,
                    ManufacturerId = 1,
                });

            modelBuilder.Entity<Product>()
                .HasData(new Product
                {
                    ProductId = 113375066,
                    ProductName = "Motorola Edge 20 8/128GB Czarny",
                    Price = 1749M,
                    ManufacturerId = 1,
                });

            modelBuilder.Entity<Product>()
                .HasData(new Product
                {
                    ProductId = 138536499,
                    ProductName = "iPhone 14 Pro",
                    Price = 1749M,
                    ManufacturerId = 2,
                });
        }
    }
}
