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
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(); 
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("user");
            modelBuilder.Entity<User>()
                .Property(u => u.Blocked)
                .HasDefaultValue(false);
            modelBuilder.Entity<User>()
                .Property(u => u.Deleted)
                .HasDefaultValue(false);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.category)
                .HasForeignKey(p => p.CategoryId);
            modelBuilder.Entity<Product>()
                .Property(p=>p.OfferPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Product>()
                .Property(p => p.RealPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId);
            modelBuilder.Entity<CartItems>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId);
           
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.RelatedOrder)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);
            modelBuilder.Entity<Address>()
                .HasKey(a => a.AddressId);
            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }
}
