using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Exceptions;
using Northwind.BusinessLogicServices.Interfaces.Models;
using Northwind.BusinessLogicServices.Interfaces.Models.Products;
using Northwind.Common.Extensions;
using Northwind.DataAccess.Interfaces;
using Northwind.DataAccess.Interfaces.Context;
using Northwind.DataAccess.Interfaces.Extensions.Quaryable;
using ProductBL = Northwind.BusinessLogicServices.Interfaces.Models.Products.Product;
using ProductDTO = Northwind.DataAccess.Interfaces.Models.Products;


namespace Northwind.BusinessLogicServices
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsContext _productsContext;
        private readonly IMapper _mapper;

        public ProductsService(IProductsContext productsContext, ICategoriesContext categoriesContext, IMapper mapper,
            ISuppliersContext suppliersContext)
        {
            _productsContext = productsContext ?? throw new ArgumentNullException(nameof(productsContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TMapTo> CreateProductAsync<TMapTo>(ProductCreate productCreate)
            where TMapTo : ProductBL
        {
            try
            {
                var mappedDtoModel = _mapper.Map<ProductCreate, ProductDTO>(productCreate);

                var createdEntity = await _productsContext.Products.AddAsync(mappedDtoModel);

                if (createdEntity is null)
                {
                    throw new CreateOperationException(
                        $"Can not create entity {typeof(ProductDTO)} from parameter {nameof(productCreate)}");
                }

                await _productsContext.SaveChangesAsync();

                var mappedCreatedEntity = _mapper.Map<ProductDTO, TMapTo>(createdEntity);

                return mappedCreatedEntity;
            }
            catch (Exception ex)
            {
                throw new CreateOperationException("Some error occured while creating new product", ex);
            }
        }

        public async Task<IEnumerable<TMapTo>> GetProductsAsync<TMapTo>(int? count = null)
            where TMapTo : ProductBL
        {
            var dbProducts = await _productsContext.Products.Take(count).ToListAsync();

            var mappedProducts = dbProducts.Select(p => _mapper.Map<ProductDTO, TMapTo>(p)).ToList();

            return mappedProducts;
        }

        public async Task<IEnumerable<TMapTo>> GetProductsWithRelatedAsync<TMapTo>(
            string[] relatedPropertiesToLoadNames, int? count = null)
            where TMapTo : ProductBL
        {
            if (relatedPropertiesToLoadNames is null)
            {
                throw new ArgumentOutOfRangeException(
                    $"Names of related entities cannot be null: parameter {nameof(relatedPropertiesToLoadNames)}");
            }

            var queryableProducts = _productsContext.Products.AsQueryable<ProductDTO>();

            foreach (var relatedPropertyName in relatedPropertiesToLoadNames)
            {
                queryableProducts = queryableProducts.Include<ProductDTO>(relatedPropertyName);
            }

            var dbProducts = await queryableProducts.Take(count).ToListAsync();

            var mappedProducts = dbProducts.Select(p => _mapper.Map<ProductDTO, TMapTo>(p)).ToList();

            return mappedProducts;
        }

        public async Task<TMapTo> UpdateProductAsync<TMapTo>(ProductUpdate productToUpdate)
            where TMapTo : ProductBL
        {
            try
            {
                var product = await this.GetProductByIdAsync(productToUpdate.ProductId);

                var mappedDtoModel = _mapper.Map<ProductUpdate, ProductDTO>(productToUpdate, product);

                var updatedModel = _productsContext.Products.Update(mappedDtoModel);

                if (updatedModel is null)
                {
                    throw new UpdateOperationException(
                        $"Can not update entity {typeof(ProductDTO)} using data from object parameter {nameof(productToUpdate)}");
                }

                await _productsContext.SaveChangesAsync();

                return _mapper.Map<TMapTo>(updatedModel);
            }
            catch (Exception ex)
            {
                throw new UpdateOperationException(
                    "Some error occured while updating product. Updated changes won't be applied", ex);
            }
        }

        public async Task<TMapTo> GetProductByIdAsync<TMapTo>(int id)
            where TMapTo : ProductBL
        {
            var foundProduct = await this.GetProductByIdAsync(id);

            return _mapper.Map<TMapTo>(foundProduct);
        }

        public async Task<TMapTo> PurgeProductAsyncById<TMapTo>(int id)
            where TMapTo : ProductBL
        {
            try
            {
                var productToPurgeDTO = _productsContext.Products.First(p => p.ProductId == id);

                if (productToPurgeDTO is null)
                {
                    throw new IdNotFoundException($"Can not find product with id {id}");
                }

                var purged = _productsContext.Products.Remove(productToPurgeDTO);

                var result = await _productsContext.SaveChangesAsync();

                if (result == 0)
                {
                    throw new DeleteOperationException($"Deleting product with id {id} was not performed");
                }

                return _mapper.Map<TMapTo>(purged);
            }
            catch (Exception ex)
            {
                throw new DeleteOperationException($"Some error occured while deleting product with id {id}", ex);
            }
        }

        private async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            if (!_productsContext.Products.Any(p => p.ProductId == productId))
            {
                throw new IdNotFoundException($"Product with id: {productId} not found.");
            }

            var productsWithCorrespondedId =
                await _productsContext.Products.Where(p => p.ProductId == productId).ToArrayAsync();

            if (productsWithCorrespondedId.Length > 1)
            {
                throw new DuplicatesFoundException(
                    $"More than one entity of type {typeof(ProductDTO)} with id {productId} where found");
            }

            var foundProduct = productsWithCorrespondedId[0];

            return foundProduct;
        }
    }
}
