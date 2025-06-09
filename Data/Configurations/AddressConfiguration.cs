using Foodkart.Models.Entities.Main;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Foodkart.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(a => a.AddressId);

            // Configure other properties as needed
            builder.Property(a => a.Place)
                   .IsRequired()
                   .HasMaxLength(100);

            // User relationship is already configured in UserConfiguration
        }
    }
}
