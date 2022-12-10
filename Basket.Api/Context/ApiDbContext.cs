using ElectroShop.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Basket.Api.Context
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options) { }

        public DbSet<UserBasket> UserBaskets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserBasket>()
                .HasData(new UserBasket
                {
                    UserId = 1,
                    BasketItems = new List<BasketItem>() 
                    {
                        new BasketItem() { ProductId = 139108829, Amount = 1, Price = 1749M }
                    }
                });
        }
    }
}
