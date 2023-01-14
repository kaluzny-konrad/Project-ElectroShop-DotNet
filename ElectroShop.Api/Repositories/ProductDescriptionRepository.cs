using ElectroShop.Api.Context;
using ElectroShop.Shared.Domain;

namespace ElectroShop.Api.Repositories;

public interface IProductDescriptionRepository
{
    IEnumerable<ProductDescription> GetProductDescriptions();
    ProductDescription? GetProductDescription(int productId);
}

public class ProductDescriptionRepository : IProductDescriptionRepository
{
    private readonly ApiDbContext _dbContext;

    public ProductDescriptionRepository(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ProductDescription? GetProductDescription(int productId)
    {
        return _dbContext.ProductDescriptions.FirstOrDefault(pd => pd.ProductId == productId);
    }

    public IEnumerable<ProductDescription> GetProductDescriptions()
    {
        return _dbContext.ProductDescriptions;
    }
}
