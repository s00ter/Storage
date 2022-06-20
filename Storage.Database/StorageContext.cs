using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Storage.Database.Entities;
using Storage.Database.Entities.Interfaces;
using Storage.Database.Entities.ProductInfos;
using Storage.Database.Entities.Products;

namespace Storage.Database
{
    public class StorageContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInfo> ProductInfo { get; set; }
        public DbSet<DeletedProduct> ProductBasket { get; set; }
        public DbSet<DeletedProductInfo> ProductInfoBasket { get; set; }

        public StorageContext(DbContextOptions<StorageContext> contextOptionsBuilder) : base(contextOptionsBuilder)
        {
            Database.EnsureCreated();

            SavingChanges += OnSavingChanges;
        }

        private void OnSavingChanges(object sender, SavingChangesEventArgs e)
        {
            var now = new Lazy<DateTime>(DateTime.Now);
            var addedEntities = ChangeTracker.Entries()
                .Where(changedEntity => changedEntity.State == EntityState.Added && changedEntity.Entity is ITrackable)
                .Select(addedEntity => addedEntity.Entity)
                .OfType<ITrackable>();

            foreach (var newEntity in addedEntities)
            {
                newEntity.CreatedAt = now.Value;
            }
        }
    }
}