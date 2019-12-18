using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Northwind.BusinessLogicServices;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Models;
using Northwind.BusinessLogicServices.Interfaces.Models.Categories;
using Northwind.BusinessLogicServices.Interfaces.Models.Products;
using Northwind.Web.MVC.Controllers;
using Northwind.Web.MVC.Utilities.Logging.LoggerAdapter;
using Xunit;
using ConfigurationProvider = Northwind.Web.MVC.Utilities.Configuration.ConfigurationProvider;
using IConfigurationProvider = Northwind.Web.MVC.Utilities.Configuration.IConfigurationProvider;

namespace Northwind.Web.MVC.Tests.UnitTests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductsService> _mockProdService;
        private readonly Mock<ICategoriesService> _mockCatService;
        private readonly Mock<ISuppliersService> _mockSupService;
        private readonly Mock<ILoggerAdapter<ProductsController>> _mockControllerLogger;
        private readonly Mock<IConfigurationProvider> _mockConfigProvider;

        private readonly ProductsController _productsController;

        public ProductsControllerTests()
        {
            _mockConfigProvider = new Mock<IConfigurationProvider>();
            _mockControllerLogger = new Mock<ILoggerAdapter<ProductsController>>();
            _mockProdService = new Mock<IProductsService>();
            _mockCatService = new Mock<ICategoriesService>();
            _mockSupService = new Mock<ISuppliersService>();

            _productsController = new ProductsController(_mockConfigProvider.Object, _mockControllerLogger.Object, _mockProdService.Object, _mockCatService.Object, _mockSupService.Object);

        }

        [Fact]
        public async Task Index_ReturnsViewWithModel_WhenServiceReturnsRightModels()
        {
            //setup
            _mockConfigProvider.Setup(c => c.GetCountOfProductsToShow()).Returns(It.IsAny<int?>());
            _mockProdService.Setup(ps =>
                    ps.GetProductsWithRelatedAsync<ProductList>(It.IsAny<string[]>(), It.IsAny<int?>()))
                .ReturnsAsync(this.GetTestProductLists());

            //act
            var actual = await _productsController.Index();

            //assert
            var viewResult = Assert.IsType<ViewResult>(actual);
            var model = Assert.IsType<List<ProductList>>(viewResult.ViewData.Model);
        }

        [Theory]
        [InlineData(null)]
        public async Task Edit_ReturnsNotFound_WhenIdDoesntHaveValue(int? id)
        {
            var actual = await _productsController.Edit(id);

            var result = Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task EditWithModel_ReturnsCurrentView_WhenModelIsNotValid()
        {
            _mockCatService.Setup(cs => cs.GetCategoriesAsync<Category>(It.IsAny<int?>()))
                .ReturnsAsync(this.GetTestCategories());
            _mockSupService.Setup(ss => ss.GetSuppliersAsync<Supplier>(It.IsAny<int?>()))
                .ReturnsAsync(this.GetTestSuppliers());

            _productsController.ModelState.AddModelError("name", "Product name is required");


            var viewModel = new ProductUpdate();

            var actual = await _productsController.Edit(It.IsAny<int>(), viewModel);

            var result = Assert.IsType<ViewResult>(actual);
            var model = Assert.IsAssignableFrom<ProductUpdate>(result.ViewData.Model);
        }

        [Fact]
        public async Task Create_ReturnsCorrectView_WhenRequested()
        {
            _mockCatService.Setup(cs => cs.GetCategoriesAsync<Category>(It.IsAny<int?>()))
                .ReturnsAsync(this.GetTestCategories());
            _mockSupService.Setup(ss => ss.GetSuppliersAsync<Supplier>(It.IsAny<int?>()))
                .ReturnsAsync(this.GetTestSuppliers());

            var actual = await _productsController.Create();

            var result = Assert.IsType<ViewResult>(actual);
        }

        [Fact]
        public async Task Create_ReturnsCurrentViewWithModel_WhenModelStateIsNotValid()
        {
            _mockCatService.Setup(cs => cs.GetCategoriesAsync<Category>(It.IsAny<int?>()))
                .ReturnsAsync(this.GetTestCategories());
            _mockSupService.Setup(ss => ss.GetSuppliersAsync<Supplier>(It.IsAny<int?>()))
                .ReturnsAsync(this.GetTestSuppliers());


            _productsController.ModelState.AddModelError("name", "Product name is required");

            var viewModel = new ProductCreate();

            var actual = await _productsController.Create(viewModel);

            var view = Assert.IsType<ViewResult>(actual);
            var model = Assert.IsType<ProductCreate>(view.ViewData.Model);
        }

        private IEnumerable<ProductList> GetTestProductLists()
        {
            return new List<ProductList> {new ProductList()};
        }

        private IEnumerable<Category> GetTestCategories()
        {
            return new List<Category> {new Category()};
        }

        private IEnumerable<Supplier> GetTestSuppliers()
        {
            return new List<Supplier> {new Supplier()};
        }
    }
}
