using AutoMapper;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Models;
using Northwind.Common.Extensions;
using Northwind.DataAccess.Interfaces.Context;
using Northwind.DataAccess.Interfaces.Extensions.Quaryable;
using Northwind.DataAccess.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.BusinessLogicServices.Interfaces.Exceptions;
using Northwind.BusinessLogicServices.Interfaces.Models.Categories;

namespace Northwind.BusinessLogicServices
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesContext _categoriesContext;
        private readonly IMapper _mapper;

        public CategoriesService(ICategoriesContext categoriesContext, IMapper mapper)
        {
            _categoriesContext = categoriesContext ?? throw new ArgumentNullException(nameof(categoriesContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<TMapTo>> GetCategoriesAsync<TMapTo>(int? count = null)
           where TMapTo : Category
        {
            try
            {
                var dbCategories = await _categoriesContext.Categories.Take(count).ToListAsync();

                var mappedCategories = dbCategories.Select(p => _mapper.Map<Categories, TMapTo>(p)).ToList();

                return mappedCategories;
            }
            catch (Exception ex)
            {
                throw new GetOperationException($"Error occured while performing get operation. See details", ex);
            }
        }

        public async Task<TMapTo> GetCategoryByIdAsync<TMapTo>(int id) where TMapTo : Category
        {
            try
            {
                var dbCategory = await _categoriesContext.Categories.Where(c => c.CategoryId == id).ToArrayAsync();

                if (dbCategory[0] is null)
                {
                    throw new IdNotFoundException($"Could not found category with id {id}");
                }

                if (dbCategory.Length > 1)
                {
                    throw new DuplicatesFoundException($"More than one categories with id {id} were found");
                }

                var mappedCategory = _mapper.Map<Categories, TMapTo>(dbCategory[0]);

                return mappedCategory;
            }
            catch (Exception ex)
            {
                throw new GetOperationException($"Error occured while performing get operation. See details", ex);
            }
        }

        public async Task<TMapTo> UpdateCategoryAsync<TMapTo>(CategoryUpdate categoryUpdate) where TMapTo : Category
        {
            try
            {
                if (categoryUpdate is null)
                {
                    throw new ArgumentNullException("categoryUpdate");
                }

                var dbCategories = await _categoriesContext.Categories
                    .Where(dc => dc.CategoryId == categoryUpdate.CategoryId).ToArrayAsync();

                if (dbCategories[0] is null)
                {
                    throw new IdNotFoundException($"Category with id {categoryUpdate.CategoryId} was not found");
                }

                if (dbCategories.Length > 1)
                {
                    throw new DuplicatesFoundException(
                        $"Found more than one category with id {categoryUpdate.CategoryId}");
                }

                var updatedDbCategory = _mapper.Map(categoryUpdate, dbCategories[0], typeof(CategoryUpdate), typeof(Categories));

                var result = await _categoriesContext.SaveChangesAsync();

                var mappedUpdatedCategory = _mapper.Map<Categories, TMapTo>(updatedDbCategory as Categories);

                return mappedUpdatedCategory;
            }
            catch (Exception ex)
            {
                throw new UpdateOperationException("Update operation for categories failed.", ex);
            }
        }

        public async Task<byte[]> GetCategoryImageAsync(int categoryId)
        {
            try
            {
                if (!this.IsCategoryExists(categoryId))
                {
                    throw new IdNotFoundException($"No category with id was found. Id : {categoryId}");
                }

                var categoryImage = await _categoriesContext.Categories.AsQueryable()
                    .Where(c => c.CategoryId == categoryId).Select(c => new {catImage = c.Picture}).ToArrayAsync();

                if (categoryImage is null)
                {
                    throw new GetOperationException(
                        $"Some error occurred while getting image of category with id {categoryId}");
                }

                if (categoryImage.Length > 1)
                {
                    throw new DuplicatesFoundException(
                        $"There are more than one category with id {categoryId} were found");
                }

                var imageAsBytes = categoryImage[0].catImage;

                return imageAsBytes;
                //var fixedImageAsBytes = new byte[imageAsBytes.Length - 78];

                //Array.Copy(imageAsBytes, 78, fixedImageAsBytes, 0, imageAsBytes.Length - 78);

                //return fixedImageAsBytes;
            }
            catch (Exception ex)
            {
                throw new GetOperationException($"Error occured while performing get operation. See details", ex);
            }
        }

        public async Task UpdateCategoryImageByIdAsync(int categoryId, byte[] imageAsBytes)
        {
            var categoryToUpdate = await _categoriesContext.Categories.Where(c => c.CategoryId == categoryId)?.ToArrayAsync();

            if (categoryToUpdate[0] is null)
            {
                throw new IdNotFoundException($"Can not find category with id {categoryId}");
            }

            categoryToUpdate[0].Picture = imageAsBytes;

            await _categoriesContext.SaveChangesAsync();
        }

        private bool IsCategoryExists(int categoryId) =>
            _categoriesContext.Categories.Any(c => c.CategoryId == categoryId);
    }
}
