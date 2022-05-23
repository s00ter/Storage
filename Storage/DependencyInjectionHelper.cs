using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Storage.Database;
using Storage.Database.Constants;

namespace Storage
{
    public static class DependencyInjectionHelper
    {
        public static IServiceCollection ConfigureDbContext(this IServiceCollection services)
        {
            services.AddDbContext<StorageContext>(x =>
                x.UseSqlite($"DataSource = {DatabaseConstants.DatabaseFileName}")
            );

            return services;
        }
    }
}