using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Northwind.Web.MVC.Filters.ActionFilters;
using Northwind.Web.MVC.Models;
using Northwind.Web.MVC.Models.Errors;
using Northwind.Web.MVC.Utilities.Logging.LoggerAdapter;

namespace Northwind.Web.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoggerAdapter<HomeController> _logger;

        public HomeController(ILoggerAdapter<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Default()
        {
            return RedirectToAction("Index");
        }

        [TypeFilter(typeof(LoggingActionFilter), Arguments = new object[]{true})]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                var exceptionFeature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();

                if (exceptionFeature != null)
                {
                    var exceptionThatOccurred = exceptionFeature.Error;

                    _logger.LogError("Redirecting to Home/Error. Error occured : ", exceptionThatOccurred);

                    return View(new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        Error = exceptionThatOccurred.Message,
                        StackTrace = exceptionThatOccurred.StackTrace
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured : ", ex);
            }

            return View();
        }
    }
}
