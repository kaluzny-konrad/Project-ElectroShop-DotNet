using ElectroShop.App.Entities;

namespace ElectroShop.App.Services
{
    public interface IManufacturerService
    {
        IEnumerable<Manufacturer> GetManufacturers();
        Manufacturer? GetManufacturer(int manufacturerId);
    }

    public class ManufacturerService : IManufacturerService
    {
        public Manufacturer? GetManufacturer(int manufacturerId)
        {
            return GetManufacturers().FirstOrDefault(m => m.ManufacturerId == manufacturerId);
        }

        public IEnumerable<Manufacturer> GetManufacturers()
        {
            return new List<Manufacturer>()
            {
                new Manufacturer
                {
                    ManufacturerId = 1,
                    ManufacturerName = "Motorola"
                },
                new Manufacturer
                {
                    ManufacturerId = 2,
                    ManufacturerName = "Apple"
                },
            };
        }
    }
}
