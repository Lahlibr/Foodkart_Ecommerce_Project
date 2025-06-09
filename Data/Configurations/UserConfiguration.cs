using System.Reflection.Emit;
using Foodkart.Models.Entities.Carts;
using Foodkart.Models.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodkart.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Set default values
            builder.HasIndex(u => u.Email).IsUnique();


            builder.Property(u => u.Role).HasDefaultValue("user");
            builder.Property(u => u.Blocked).HasDefaultValue(false);
            builder.Property(u => u.Deleted).HasDefaultValue(false);

            // Configure table name (if different from entity name)
            builder.ToTable("Users"); // Consider using "Users" instead of "Registration"

            // Relationships with restricted delete behavior
            

            builder.HasOne(u => u.Carts)
                   .WithOne(c => c.User)
                   .HasForeignKey<Cart>(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Wishlists)
                   .WithOne(w => w.users)
                   .HasForeignKey(w => w.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Addresses)
                   .WithOne(a => a.User)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
