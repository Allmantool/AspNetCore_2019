using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.BusinessLogicServices.Interfaces.Models.Categories;

namespace Northwind.BusinessLogicServices.Interfaces
{
    public interface ICategoriesService
    {
        Task<IEnumerable<TMapTo>> GetCategoriesAsync<TMapTo>(int? count = null)
         where TMapTo : Category;

        Task<TMapTo> GetCategoryByIdAsync<TMapTo>(int id)
            where TMapTo : Category;

        Task<TMapTo> UpdateCategoryAsync<TMapTo>(CategoryUpdate categoryEdit)
            where TMapTo : Category;

        Task<byte[]> GetCategoryImageAsync(int categoryId);

        Task UpdateCategoryImageByIdAsync(int categoryId, byte[] imageAsBytes);
    }
}
