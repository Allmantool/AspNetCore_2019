using AutoMapper;
using Northwind.BusinessLogicServices.Interfaces.Models;
using Northwind.DataAccess.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Northwind.BusinessLogicServices.Interfaces.Models.Categories;

namespace Northwind.BusinessLogicServices.Models.MappingProfiles
{
    public class CategoriesMappingProfile : Profile
    {
        public CategoriesMappingProfile()
        {
            this.CreateMap<Categories, CategoryList>();
            this.CreateMap<Categories, Category>();
            this.CreateMap<Categories, CategoryUpdate>();
            this.CreateMap<CategoryUpdate, Categories>();
        }
    }
}
