using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Northwind.Web.MVC.Controllers;
using Northwind.Web.MVC.Models;
using Northwind.Web.MVC.Utilities.Logging.LoggerAdapter;
using Xunit;

namespace Northwind.Web.MVC.Tests.UnitTests
{
    public class HomeControllerTests
    {
        private readonly Mock<ILoggerAdapter<HomeController>> _mockLogger;
        private readonly HomeController _homeController;
        
        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILoggerAdapter<HomeController>>();
            _homeController = new HomeController(_mockLogger.Object);
        }

        [Fact]
        public void IndexGet_ReturnsView_WhenDefaultRequest()
        {
            var result = _homeController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ErrorGet_ReturnsView_WhenNoExceptionThrown()
        {
            _mockLogger.Setup(l => l.LogError(It.IsAny<string>()));

            var result = _homeController.Error();

            var viewResult = Assert.IsType<ViewResult>(result);
        }
    }
}
