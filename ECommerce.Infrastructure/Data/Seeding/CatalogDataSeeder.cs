using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Seeding
{
    public class CatalogDataSeeder(StoreDbContext dbContext) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);
                if (PendingMigrations.Any())
                    await dbContext.Database.MigrateAsync(ct);

                // D:Route\C46\09 API\Project\ECommerce.API\bin\Debug\net8.0\DataSeed
                var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");

                await SeedIfEmptyAsync<ProductBrand, int>(seedRoot, "brands.json", ct);
                await SeedIfEmptyAsync<ProductType, int>(seedRoot, "types.json", ct);
                await SeedIfEmptyAsync<Product, int>(seedRoot, "products.json", ct); 

                int result = await dbContext.SaveChangesAsync(ct);

                if (result > 0)
                    Console.WriteLine($"Data Seeded Successfully");
                else
                    Console.WriteLine("Failed to Seed Data ... ");
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task SeedIfEmptyAsync<T, TKey>(string rootPath, string fileName, CancellationToken ct) where T : BaseEntity<TKey>
        {
            if (await dbContext.Set<T>().AnyAsync())
            {
                Console.WriteLine("Seeding Failed ... Data Exists in The Table");
                return;
            }

            var filePath = Path.Combine(rootPath, fileName);

            if (!File.Exists(filePath))
                return;

            using var fileStream = File.OpenRead(filePath);

            var items = await JsonSerializer.DeserializeAsync<List<T>>(fileStream);

            if (items?.Any() ?? false)
                dbContext.Set<T>().AddRange(items);
        }
    }
}
