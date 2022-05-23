using Microsoft.EntityFrameworkCore;
using Storage.Database.Entities;

namespace Storage.Database
{
    public class StorageContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public StorageContext(DbContextOptionsBuilder<StorageContext> contextOptionsBuilder)
        {

        }
    }
}