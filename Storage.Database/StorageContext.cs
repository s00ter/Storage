using Microsoft.EntityFrameworkCore;
using Storage.Database.Entities;

namespace Storage.Database
{
    public class StorageContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInfo> ProductInfo { get; set; }

        public StorageContext(DbContextOptions<StorageContext> contextOptionsBuilder) : base(contextOptionsBuilder)
        {
            Database.EnsureCreated();
        }
    }
}