using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Models.Products;
using Northwind.Web.MVC.Models.WebApiResults;
using Northwind.Web.MVC.Utilities.Logging.LoggerAdapter;

namespace Northwind.Web.MVC.Controllers.Api
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly ILoggerAdapter<ProductsController> _logger;

        public ProductsController(IProductsService productsService, ILoggerAdapter<ProductsController> logger)
        {
            _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<WebApiResult> GetProducts([FromQuery]int? count)
        {
            try
            {
                var products = await _productsService.GetProductsAsync<Product>(count);

                var result = new WebApiResult
                {
                    Data = products,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success"
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting products", null);

                var result = new WebApiResult
                {
                    Data = null,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Error"
                };

                return result;
            }
        }


        [HttpPost]
        public async Task<WebApiResult> CreateProduct([FromBody] ProductCreate productCreate)
        {
            try
            {
                var createdProduct = await _productsService.CreateProductAsync<Product>(productCreate);

                var result = new WebApiResult
                {
                    Data = createdProduct,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success"
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured while creating product with id {productCreate.ProductId}", null);

                var exceptionMessageBuilder = new StringBuilder($"Error: {ex.Message};");

                var innerException = ex?.InnerException;

                while (innerException != null)
                {
                    exceptionMessageBuilder.Append($"{ex.InnerException};");

                    innerException = innerException.InnerException;
                }

                var result = new WebApiResult
                {
                    Data = null,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = exceptionMessageBuilder.ToString()
                };

                return result;
            }
        }

        [HttpPut]
        public async Task<WebApiResult> UpdateProduct([FromBody] ProductUpdate productUpdate)
        {
            try
            {
                var updatedProduct = await _productsService.UpdateProductAsync<Product>(productUpdate);

                var result = new WebApiResult
                {
                    Data = updatedProduct,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success"
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured while updating product with id {productUpdate.ProductId}", null);

                var exceptionMessageBuilder = new StringBuilder($"Error: {ex.Message};");

                var innerException = ex?.InnerException;

                while (innerException != null)
                {
                    exceptionMessageBuilder.Append($"{ex.InnerException};");

                    innerException = innerException.InnerException;
                }

                var result = new WebApiResult
                {
                    Data = null,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = exceptionMessageBuilder.ToString()
                };

                return result;
            }
        }

        [HttpDelete]
        public async Task<WebApiResult> PurgeProductById([FromQuery]int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return new WebApiResult
                    {
                        Data = null,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "No id provided to delete product"
                    };
                }

                var purgedProduct = await _productsService.PurgeProductAsyncById<Product>(id.Value);

                var result = new WebApiResult
                {
                    Data = purgedProduct,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success"
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured while purging product with id {id.Value}", null);

                var exceptionMessageBuilder = new StringBuilder($"Error: {ex.Message};");

                var innerException = ex?.InnerException;

                while (innerException != null)
                {
                    exceptionMessageBuilder.Append($"{ex.InnerException};");

                    innerException = innerException.InnerException;
                }

                var result = new WebApiResult
                {
                    Data = null,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = exceptionMessageBuilder.ToString()
                };

                return result;
            }
        }
    }
}