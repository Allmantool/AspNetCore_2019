using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.DataAccess.Interfaces.Context;
using Northwind.DataAccess;
using System;
using Northwind.DataAccess.Context;

namespace Northwind.DataAccess.ServicesConfiguration
{
    public static class DataAcessConfiguration
    {
        public static void ConfigureDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NorthwindContext>(opts => opts.UseSqlServer(configuration.GetConnectionString(typeof(NorthwindContext)?.Name)));

            services.AddTransient<IProductsContext, ProductsContext>();
            services.AddTransient<ICategoriesContext, CategoriesContext>();
            services.AddTransient<ISuppliersContext, SuppliersContext>();
        }
    }
}
