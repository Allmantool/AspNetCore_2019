using System;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Models.Categories;
using Northwind.Web.MVC.Models;
using Northwind.Web.MVC.Utilities;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Northwind.Common.Extensions;
using Northwind.Web.MVC.Filters.ActionFilters;
using Northwind.Web.MVC.Models.Categories;
using Northwind.Web.MVC.Utilities.Extensions;
using Northwind.Web.MVC.Utilities.Extensions.StaticFilesExtensions;
using Northwind.Web.MVC.Utilities.Logging.LoggerAdapter;

namespace Northwind.Web.MVC.Controllers
{
    [Authorize(AuthenticationSchemes = AzureADDefaults.AuthenticationScheme)]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly ILoggerAdapter<CategoriesController> _logger;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoriesService categoriesService, ILoggerAdapter<CategoriesController> logger,
            IMapper mapper)
        {
            _categoriesService = categoriesService ?? throw new ArgumentNullException(nameof(categoriesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index()
        {
            var categoriesList = await _categoriesService.GetCategoriesAsync<CategoryList>();
            
            return View(categoriesList);
        }

        public async Task<IActionResult> Edit([FromQuery(Name="categoryId")]int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return NotFound();
                }

                var categoryToEdit = await _categoriesService.GetCategoryByIdAsync<CategoryUpdate>(id.Value);

                var categoryEditViewModel = _mapper.Map<CategoryUpdate, CategoryEditViewModel>(categoryToEdit);

                return View(categoryEditViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromQuery(Name = "categoryId")]int? id, [Bind("CategoryId","CategoryName","Description", "Picture")]CategoryEditViewModel edit)
        {
            if (!this.ModelState.IsValid)
            {
                _logger.LogWarning("Category model to edit is invalid. Redirecting to Index");
                
                return View(edit);
            }
            try
            {
                _logger.LogInformation($"Started updating category model with id : {edit.CategoryId} ...");

                var mappedCategory = _mapper.Map<CategoryEditViewModel, CategoryUpdate>(edit);

                var updatedCategory = await _categoriesService.UpdateCategoryAsync<CategoryUpdate>(mappedCategory);

                var editViewModel = _mapper.Map<CategoryUpdate, CategoryEditViewModel>(updatedCategory);
                
                _logger.LogInformation($"Successfully finished updating category model with id : {edit.CategoryId} !");

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some error occured while processing request. See details.",null);

                return View(edit);
            }
        }

        [TypeFilter(typeof(LoggingActionFilter), Arguments = new object[] { true })]
        public async Task<IActionResult> GetImage(int? categoryId)
        {
            if (!categoryId.HasValue)
            {
                throw new ArgumentNullException($"Category id parameter is null");
            }

            var categoryImageAsBytes = await _categoriesService.GetCategoryImageAsync(categoryId.Value);

            await using var imageStream = new MemoryStream(categoryImageAsBytes);
            
            var categoryImage = Image.FromStream(imageStream);
            
            var imageFormat = categoryImage.GetImageFormat();

            var imageExtension = imageFormat.ToString().ToLowerInvariant();

            var imageContentType = FileExtensions.GetContentTypeByExtension(imageExtension.ToLowerInvariant());

            var imageFileName = this.GenerateFileName(categoryId.Value.ToString(), imageExtension);
            
            return File(categoryImageAsBytes, imageContentType, imageFileName);
        
        }

        private string GenerateFileName(string name, string extension) => $"{Guid.NewGuid()}-{name}.{extension}";
    }
}
