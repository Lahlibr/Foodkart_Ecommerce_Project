using Foodkart.Models.Entities.Carts;
using Foodkart.Models.Entities.Main;
using Foodkart.Models.Entities.Orders;
using Foodkart.Models.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace Foodkart.Data
{
    public class FoodkartDbContext : DbContext
    {
        public FoodkartDbContext(DbContextOptions<FoodkartDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
       
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        //I am customizing how EF Core creates the database model for my application.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfigurationsFromAssembly(typeof(FoodkartDbContext).Assembly);
            // This will apply all configurations in the assembly where FoodkartDbContext is defined.
            // It assumes that you have created configuration classes for your entities.

        }


    }
}
