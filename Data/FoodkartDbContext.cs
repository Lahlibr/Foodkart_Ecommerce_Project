using Foodkart.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Foodkart.Data
{
    public class FoodkartDbContext : DbContext
    {
        public FoodkartDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Registration> Registration { get; set; }
        public DbSet<Login> Login { get; set; }
    }
}
