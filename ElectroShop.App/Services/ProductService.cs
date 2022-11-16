using ElectroShop.App.Entities;

namespace ElectroShop.App.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product? GetProduct(int productId);
    }

    public class ProductService : IProductService
    {
        public Product? GetProduct(int productId)
        {
            return GetProducts().FirstOrDefault(p => p.ProductId == productId);
        }

        public IEnumerable<Product> GetProducts()
        {
            return new List<Product>() {
                new Product 
                { 
                    ProductId = 139108825, 
                    ProductName = "Motorola Edge 30 Neo 8/128GB Czarny", 
                    Price = 1749F,
                    ManufacturerId = 1,
                },
                new Product
                { 
                    ProductId = 139108829, 
                    ProductName = "Motorola Edge 30 Neo 8/128GB Fioletowy", 
                    Price = 1749F,
                    ManufacturerId = 1,
                },
                new Product 
                { 
                    ProductId = 139120876, 
                    ProductName = "Motorola Edge 30 Neo 8/128GB Srebrny",
                    Price = 1749F,
                    ManufacturerId = 1,
                },
                new Product 
                { 
                    ProductId = 113375066, 
                    ProductName = "Motorola Edge 20 8/128GB Czarny",
                    Price = 1749F,
                    ManufacturerId = 1,
                },
                new Product
                { 
                    ProductId = 138536499, 
                    ProductName = "iPhone 14 Pro", 
                    Price = 6999F,
                    ManufacturerId = 2,
                },
            };
        }
    }
}
