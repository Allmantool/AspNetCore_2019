using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Models;
using Northwind.BusinessLogicServices.Models.MappingProfiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.BusinessLogicServices.ServicesConfiguration
{
    public static class BusinessLogicServicesConfigurations
    {
        public static void ConfigureBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
        { 
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<ISuppliersService, SuppliersService>();
        }
    }
}
