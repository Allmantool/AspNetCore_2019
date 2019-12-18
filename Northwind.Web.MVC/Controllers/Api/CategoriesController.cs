using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Models.Categories;
using Northwind.Common.Extensions;
using Northwind.Web.MVC.Models.WebApiResults;
using Northwind.Web.MVC.Utilities.Extensions.StaticFilesExtensions;
using Northwind.Web.MVC.Utilities.Logging.LoggerAdapter;

namespace Northwind.Web.MVC.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        private readonly ILoggerAdapter<CategoriesController> _logger;

        public CategoriesController(ICategoriesService categoriesService,
            ILoggerAdapter<CategoriesController> logger)
        {
            _categoriesService = categoriesService ?? throw new ArgumentNullException(nameof(categoriesService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<WebApiResult> GetCategories([FromQuery]int? count)
        {
            try
            {
                var categories = await _categoriesService.GetCategoriesAsync<CategoryList>(count);

                var result = new WebApiResult
                {
                    Data = categories,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success"
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting categories", null);

                var result = new WebApiResult
                {
                    Data = null,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Error"
                };

                return result;
            }
        }

        [HttpGet("image")]
        public async Task<IActionResult> GetCategoryImageById([FromQuery] int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return NotFound();
                }

                var categoryImageAsBytes = await _categoriesService.GetCategoryImageAsync(id.Value);

                await using var imageStream = new MemoryStream(categoryImageAsBytes);

                var categoryImage = Image.FromStream(imageStream);

                var imageFormat = categoryImage.GetImageFormat();

                var imageExtension = imageFormat.ToString().ToLowerInvariant();

                var imageContentType = FileExtensions.GetContentTypeByExtension(imageExtension.ToLowerInvariant());

                var imageFileName = this.GenerateFileName(id.Value.ToString(), imageExtension);
                
                return File(categoryImageAsBytes, imageContentType, imageFileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting categories", null);

                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("image")]
        public async Task<IActionResult> PostCategoryImage([FromForm]IFormFile categoryImage, [FromQuery] int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return NotFound();
                }

                if (categoryImage is null || categoryImage.Length == 0)
                {
                    throw new Exception("Category image is null or empty.");
                }

                await using var mStream = new MemoryStream();

                await categoryImage.CopyToAsync(mStream);

                await _categoriesService.UpdateCategoryImageByIdAsync(id.Value, mStream.ToArray());

                return Ok("Image was updated");
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured",ex);
            }
        }

        private string GenerateFileName(string name, string extension) => $"{Guid.NewGuid()}-{name}.{extension}";
    }
}