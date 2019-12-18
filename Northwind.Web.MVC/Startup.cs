using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Northwind.BusinessLogicServices;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Models.Caches;
using Northwind.CompositionRoot;
using Northwind.Web.MVC.Filters.ActionFilters;
using Northwind.Web.MVC.Middlewares;
using Northwind.Web.MVC.Utilities.Extensions;
using Northwind.Web.MVC.Utilities.Logging.LoggerAdapter;
using ConfigurationProvider = Northwind.Web.MVC.Utilities.Configuration.ConfigurationProvider;
using IConfigurationProvider = Northwind.Web.MVC.Utilities.Configuration.IConfigurationProvider;

namespace Northwind.Web.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddMvc(opts =>
            {
                opts.Filters.Add<LoggingActionFilter>();
                opts.RespectBrowserAcceptHeader = true;

            }).AddNewtonsoftJson();

            services.AddCors();
            services.AddSwaggerDocument();

            services.ConfigureServices(this.Configuration);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("Northwind")).ToArray());

            services.AddSingleton<IExceptionHandlerFeature, ExceptionHandlerFeature>();
            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMemoryCache(opts => new MemoryCacheOptions{ExpirationScanFrequency = TimeSpan.FromMilliseconds(500)});
            services.AddSingleton<IImagesCachingService, ImagesCachingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation("In Development environment");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                logger.LogInformation("In Release environment");
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseCors(builder => builder.AllowAnyOrigin());

            app.UseHsts();
            app.UseHttpsRedirection();

            app.UseImagesCaching(new ImageCacheOptions
            {
                ExpirationTimeSpanInMilliseconds = 1000,
                MaxCountOfCachedImages = 1,
                PathToCacheDirectory = "C:/learning/cached"
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();

                endpoints.MapControllerRoute(
                    "categories_images",
                    "images/{categoryId}",
                    new {controller = "Categories", action = "GetImage"},
                    new {categoryId = new IntRouteConstraint()});
            });

        }
    }
}
