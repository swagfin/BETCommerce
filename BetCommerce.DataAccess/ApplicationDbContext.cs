using BetCommerce.Entity.Core;
using Microsoft.EntityFrameworkCore;

namespace BetCommerce.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.Database.Migrate();
        }
        //********  ENTITIES *********
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductStockCard> ProductStockCards { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}
