using AutoMapper;
using Northwind.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.BusinessLogicServices.Models.Models.MappingProfiles
{
    internal class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            this.CreateMap<Products, ProductList>()
                .ForMember(pl => pl.CategoryName, opts => opts.MapFrom(p => p.Category.CategoryName))
                .ForMember(pl => pl.SupplierName, opts => opts.MapFrom(p => p.Supplier.CompanyName));
        }

    }
}
