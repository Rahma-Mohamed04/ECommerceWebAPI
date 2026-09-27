using ECommerce.Domain.Contracts;

namespace ECommerce.API.Extensions
{
    public static class WebApplicationExtension
    {
        public static async Task<WebApplication> SeedAndMigrateDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");

            await seeder.SeedDataAsync();
            return app;
        }
    }
}
