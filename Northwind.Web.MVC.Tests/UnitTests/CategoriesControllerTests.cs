using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Exceptions;
using Northwind.BusinessLogicServices.Interfaces.Models.Categories;
using Northwind.Web.MVC.Controllers;
using Northwind.Web.MVC.Models;
using Northwind.Web.MVC.Models.Categories;
using Northwind.Web.MVC.Utilities.Logging.LoggerAdapter;
using Xunit;

namespace Northwind.Web.MVC.Tests.UnitTests
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ILoggerAdapter<CategoriesController>> _mockCatControllerLogger;
        private readonly Mock<ICategoriesService> _mockCatService;
        private readonly Mock<IMapper> _mockMapper;
        
        private readonly CategoriesController _categoriesController;

        public CategoriesControllerTests()
        {
            _mockCatControllerLogger = new Mock<ILoggerAdapter<CategoriesController>>();
            _mockCatService = new Mock<ICategoriesService>();
            _mockMapper = new Mock<IMapper>();

            _categoriesController = new CategoriesController(_mockCatService.Object, _mockCatControllerLogger.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task IndexGet_ReturnsViewResult_WhenCategoriesServiceReturnsEntitiesTypeOfCategoryList()
        {
            _mockCatService.Setup(cs => cs.GetCategoriesAsync<CategoryList>(null))
                .ReturnsAsync(GetTestCategoryListEnumerable());

            var actual = await _categoriesController.Index();

            Assert.IsType<ViewResult>(actual);
        }

        [Fact]
        public async Task IndexGet_InjectsModelIntoViewResult_WhenCategoriesServiceReturnsEntitiesTypeOfCategoryList()
        {
            _mockCatService.Setup(cs => cs.GetCategoriesAsync<CategoryList>(null))
                .ReturnsAsync(GetTestCategoryListEnumerable());

            var actual = await _categoriesController.Index();

            var viewResult = Assert.IsType<ViewResult>(actual);
            var model = Assert.IsAssignableFrom<IEnumerable<CategoryList>>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task
            IndexGet_InjectsNotNullModelIntoViewResult_WhenCategoriesServiceReturnsEntitiesTypeOfCategoryList()
        {
            _mockCatService.Setup(cs => cs.GetCategoriesAsync<CategoryList>(It.IsAny<int?>()))
                .ReturnsAsync(GetTestCategoryListEnumerable());

            var actual = await _categoriesController.Index();

            var viewResult = Assert.IsType<ViewResult>(actual);
            var model = Assert.IsAssignableFrom<IEnumerable<CategoryList>>(viewResult.ViewData.Model);

            Assert.NotNull(model);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        public async Task GetImage_ThrowsExceptionOfExceptedType_WhenCategoryIdIsNull(int? categoryId, Type expected)
        {
            _mockCatService.Setup(cs => cs.GetCategoryImageAsync(It.IsAny<int>()))
                .ReturnsAsync(It.IsAny<byte[]>());
            
            var actual = await Assert.ThrowsAsync(expected, () => _categoriesController.GetImage(categoryId));
        }

        [Theory]
        [InlineData(null)]
        public async Task Edit_ReturnsNotFound_WhenIdDoesntHaveValue(int? id)
        {
            //_mockCatService.Setup(cs => cs.GetCategoryByIdAsync<CategoryUpdate>(It.IsAny<int>()))
            //    .ReturnsAsync(It.IsAny<CategoryUpdate>());
            var actual = await _categoriesController.Edit(id);

            var result = Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task EditWithModel_ReturnsCurrentView_WhenModelIsNotValid()
        {
            _categoriesController.ModelState.AddModelError("name", "Category name is required");

            var viewModel = new CategoryEditViewModel();

            var actual = await _categoriesController.Edit(It.IsAny<int>(), viewModel);

            var result = Assert.IsType<ViewResult>(actual);
            var model = Assert.IsAssignableFrom<CategoryEditViewModel>(result.ViewData.Model);
        }

        private IEnumerable<CategoryList> GetTestCategoryListEnumerable()
        {
            yield return new CategoryList
            {
                CategoryId = 1,
                CategoryName = "TestName",
                Description = "TestDesc"
            };
        }
    }
}
