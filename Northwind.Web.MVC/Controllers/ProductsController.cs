using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Models;
using Northwind.BusinessLogicServices.Interfaces.Models.Categories;
using Northwind.BusinessLogicServices.Interfaces.Models.Products;
using Northwind.Web.MVC.Filters.ActionFilters;
using Northwind.Web.MVC.Utilities;
using Northwind.Web.MVC.Utilities.Configuration;
using Northwind.Web.MVC.Utilities.Logging.LoggerAdapter;


namespace Northwind.Web.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfigurationProvider _config;
        private readonly ILoggerAdapter<ProductsController> _logger;
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        private readonly ISuppliersService _suppliersService;

        public ProductsController(IConfigurationProvider config, ILoggerAdapter<ProductsController> logger,
            IProductsService productsService, ICategoriesService categoriesService, ISuppliersService suppliersService)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
            _categoriesService = categoriesService ?? throw new ArgumentNullException(nameof(categoriesService));
            _suppliersService = suppliersService ?? throw new ArgumentNullException(nameof(suppliersService));
        }

        public async Task<IActionResult> Index()
        {
            var countOfProductsToShow = _config.GetCountOfProductsToShow();

            var prods = await _productsService.GetProductsWithRelatedAsync<ProductList>(new[] { "Supplier", "Category"}, countOfProductsToShow);

            return View(prods);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoriesService.GetCategoriesAsync<CategoryList>();
            var suppliers = await _suppliersService.GetSuppliersAsync<Supplier>();

            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "CompanyName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] ProductCreate productToCreate)
        {
            if (ModelState.IsValid)
            {
                var createdProduct = await _productsService.CreateProductAsync<Product>(productToCreate);

                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoriesService.GetCategoriesAsync<Category>();
            var suppliers = await _suppliersService.GetSuppliersAsync<Supplier>();

            ViewData["CategoryId"] = new SelectList(categories, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(suppliers, "SupplierId", "CompanyName");

            return View(productToCreate);
        }

        public async Task<IActionResult> Edit([FromQuery]int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var categories = await _categoriesService.GetCategoriesAsync<Category>();
            var suppliers = await _suppliersService.GetSuppliersAsync<Supplier>();

            var productToEdit = await _productsService.GetProductByIdAsync<ProductUpdate>(id.Value);

            ViewData["CategoryId"] =
                new SelectList(categories, "CategoryId", "CategoryName", productToEdit.CategoryId);
            ViewData["SupplierId"] =
                new SelectList(suppliers, "SupplierId", "CompanyName", productToEdit.SupplierId);

            return View(productToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromQuery]int id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] ProductUpdate products)
        {
            if (id != products.ProductId)
            {
                _logger.LogError("Product's id from request and model are not the same");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _productsService.UpdateProductAsync<ProductUpdate>(products);

                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex,
                        $"Some error occured while processing Edit for product with id {products.ProductId}");
                }
            }

            var availableCategories = await _categoriesService.GetCategoriesAsync<Category>();
            var availableSuppliers = await _suppliersService.GetSuppliersAsync<Supplier>();

            ViewData["CategoryId"] = new SelectList(availableCategories, "CategoryId", "CategoryName", products.CategoryId);
            ViewData["SupplierId"] = new SelectList(availableSuppliers, "SupplierId", "CompanyName", products.SupplierId);

            return View(products);
        }

        
    }
}
