using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Data.Seeding;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure
{
    public static class InfrastructureServiceRegisteration
    {
        public static IServiceCollection AddInfratructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog");
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
