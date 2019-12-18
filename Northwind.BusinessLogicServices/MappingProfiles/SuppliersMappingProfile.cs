using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Northwind.BusinessLogicServices.Interfaces.Models;
using Northwind.DataAccess.Interfaces.Models;

namespace Northwind.BusinessLogicServices.MappingProfiles
{
    public class SuppliersMappingProfile : Profile
    {
        public SuppliersMappingProfile()
        {
            this.CreateMap<Suppliers, Supplier>();
        }
        
    }
}
