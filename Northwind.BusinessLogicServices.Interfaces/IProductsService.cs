using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Northwind.BusinessLogicServices.Interfaces.Models.Products;

namespace Northwind.BusinessLogicServices.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<TMapTo>> GetProductsAsync<TMapTo>(int? count = null)
            where TMapTo : Product;

        Task<IEnumerable<TMapTo>> GetProductsWithRelatedAsync<TMapTo>(string[] relatedEntitiesToLoadNames, int? count = null)
            where TMapTo : Product;

        Task<TMapTo> CreateProductAsync<TMapTo>(ProductCreate productCreate)
            where TMapTo : Product;

        Task<TMapTo> UpdateProductAsync<TMapTo>(ProductUpdate productToUpdate)
            where TMapTo : Product;

        Task<TMapTo> GetProductByIdAsync<TMapTo>(int id)
            where TMapTo : Product;

        Task<TMapTo> PurgeProductAsyncById<TMapTo>(int id)
            where TMapTo : Product;
    }
}
