using ECommerce.Application.Contracts;
using ECommerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application
{
    public static class ApplicationServicesRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(C => { }, typeof(ApplicationServicesRegisteration).Assembly);
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            return services;
        }

    }
}
