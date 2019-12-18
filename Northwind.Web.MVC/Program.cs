using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Northwind.Web.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appConfiguration = BuildConfiguration(@"appsettings.json");

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(appConfiguration)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseSerilog();
                });

        private static IConfiguration BuildConfiguration(string pathToConfigurationFile)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(pathToConfigurationFile, optional: false)
                .Build();

            return config;
        }
    }
}
