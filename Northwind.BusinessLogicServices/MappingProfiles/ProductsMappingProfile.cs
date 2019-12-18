using AutoMapper;
using Northwind.BusinessLogicServices.Interfaces.Models.Products;
using ProductBL = Northwind.BusinessLogicServices.Interfaces.Models.Products.Product;
using ProductDTO = Northwind.DataAccess.Interfaces.Models.Products;

namespace Northwind.BusinessLogicServices.Models.MappingProfiles
{
    public class ProductsMappingProfile : Profile
    {
        public ProductsMappingProfile()
        {
            this.CreateMap<ProductDTO, ProductList>()
                .ForMember(pl => pl.CategoryName, opts => opts.MapFrom(p => p.Category.CategoryName))
                .ForMember(pl => pl.SupplierName, opts => opts.MapFrom(p => p.Supplier.CompanyName));
            this.CreateMap<ProductDTO, ProductBL>();
            this.CreateMap<ProductCreate, ProductDTO>();
            this.CreateMap<ProductUpdate, ProductDTO>();
        }

    }
}
