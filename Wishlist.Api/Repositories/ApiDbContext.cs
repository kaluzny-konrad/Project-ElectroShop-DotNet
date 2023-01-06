using ElectroShop.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Wishlist.Api.Repositories
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options) { }

        public DbSet<WishlistElement> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WishlistElement>()
                .HasData(new
                {
                    Id = 1,
                    UserId = 1,
                    ProductId = 139108829,
                    AddedDate = DateTime.UtcNow,
                });
        }
    }
}
