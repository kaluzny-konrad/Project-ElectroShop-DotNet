using ElectroShop.Api.Context;
using ElectroShop.Shared.Domain;

namespace ElectroShop.Api.Repositories;

public interface IProductRepository
{
    IEnumerable<Product> GetProducts();
    Product? GetProduct(int productId);
}

public class ProductRepository : IProductRepository
{
    private readonly ApiDbContext _dbContext;

    public ProductRepository(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Product? GetProduct(int productId)
    {
        return _dbContext.Products.FirstOrDefault(p => p.ProductId == productId);
    }

    public IEnumerable<Product> GetProducts()
    {
        return _dbContext.Products;
    }
}
