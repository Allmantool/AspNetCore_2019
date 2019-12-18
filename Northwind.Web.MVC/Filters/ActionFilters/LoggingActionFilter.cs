using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Northwind.Web.MVC.Utilities.Logging.LoggerAdapter;

namespace Northwind.Web.MVC.Filters.ActionFilters
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILoggerAdapter<LoggingActionFilter> _logger;
        private readonly bool _isLogParametersOfActionMethod;

        public LoggingActionFilter(ILoggerAdapter<LoggingActionFilter> logger, bool isLogParametersOfActionMethod = false)
        {
            _logger = logger;
            _isLogParametersOfActionMethod = isLogParametersOfActionMethod;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var action = context.ActionDescriptor;

            var log = new StringBuilder();
            log.Append($"Action ended : {action.DisplayName}\n");

            if (_isLogParametersOfActionMethod)
            {
                log.Append($"Parameters : \n");

                foreach (var param in action.Parameters)
                {
                    log.Append(
                        $"Name : {param.Name} ; Type : {param.ParameterType.Name}");
                }
            }

            _logger.LogInformation(log.ToString());
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.ActionDescriptor;

            var log = new StringBuilder();
            log.Append($"Action started : {action.DisplayName}\n");

            if (_isLogParametersOfActionMethod)
            {
                log.Append($"Parameters : \n");

                foreach (var param in action.Parameters)
                {
                    log.Append(
                        $"Name : {param.Name} ; Type : {param.ParameterType.Name}");
                }
            }

            _logger.LogInformation(log.ToString());
        }
    }
}
