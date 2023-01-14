using ElectroShop.Api.Context;
using ElectroShop.Shared.Domain;

namespace ElectroShop.Api.Repositories;

public interface IManufacturerRepository
{
    IEnumerable<Manufacturer> GetManufacturers();
    Manufacturer? GetManufacturer(int manufacturerId);
}

public class ManufacturerRepository : IManufacturerRepository
{
    private readonly ApiDbContext _dbContext;

    public ManufacturerRepository(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Manufacturer? GetManufacturer(int manufacturerId)
    {
        return _dbContext.Manufacturers.FirstOrDefault(m => m.ManufacturerId == manufacturerId);
    }

    public IEnumerable<Manufacturer> GetManufacturers()
    {
        return _dbContext.Manufacturers;
    }
}
