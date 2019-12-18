using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.BusinessLogicServices.ServicesConfiguration;
using Northwind.DataAccess.ServicesConfiguration;
using System;

namespace Northwind.CompositionRoot
{
    public static class Configuration
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureBusinessLogicServices(configuration);
            services.ConfigureDataAccessServices(configuration);
        }
    }
}
