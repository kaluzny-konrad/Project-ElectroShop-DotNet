using ElectroShop.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Basket.Api.Context
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options) { }

        public DbSet<BasketItem> BasketItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BasketItem>()
                .HasData(new BasketItem {
                    BasketItemId = 1,
                    UserId = 1,
                    ProductId = 139108829,
                    Amount = 2,
                });

            modelBuilder.Entity<BasketItem>()
                .HasData(new BasketItem
                {
                    BasketItemId = 2,
                    UserId = 1,
                    ProductId = 139120876,
                    Amount = 1,
                });

            modelBuilder.Entity<BasketItem>()
                .HasData(new BasketItem
                {
                    BasketItemId = 3,
                    UserId = 1,
                    ProductId = 138536499,
                    Amount = 1,
                });

            modelBuilder.Entity<BasketItem>()
                .HasData(new BasketItem
                {
                    BasketItemId = 4,
                    UserId = 2,
                    ProductId = 139120876,
                    Amount = 1,
                });

            modelBuilder.Entity<BasketItem>()
                .HasData(new BasketItem
                {
                    BasketItemId = 5,
                    UserId = 2,
                    ProductId = 138536499,
                    Amount = 1,
                });
        }
    }
}
