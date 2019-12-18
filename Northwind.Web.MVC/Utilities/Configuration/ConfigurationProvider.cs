using System;
using Microsoft.Extensions.Configuration;
using Northwind.Web.MVC.Utilities.Logging.LoggerAdapter;

namespace Northwind.Web.MVC.Utilities.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerAdapter<ConfigurationProvider> _logger;

        public ConfigurationProvider(IConfiguration configuration, ILoggerAdapter<ConfigurationProvider> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public int? GetCountOfProductsToShow()
        {
            _logger.LogInformation("Started reading configurations \"Products\", trying to get section M..");

            var countAsString = _configuration.GetSection("Products:M").Value;

            _logger.LogInformation($"Ended reading configurations \"Products\", got value of section M : {countAsString}");

            if (int.TryParse(countAsString, out int parsedCount))
            {
                return parsedCount == 0 ? new int?() : new int?(parsedCount);
            }
            else
            {
                return new int?();
            }
        }
    }
}
