using Northwind.DataAccess.Interfaces;
using Northwind.DataAccess.Interfaces.Context;
using Northwind.DataAccess.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.DataAccess.Context
{
    public class ProductsContext : AppDomainContextBase<NorthwindContext>, IProductsContext 
    {
        public ProductsContext(NorthwindContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<Products> Products => base.GetEntitySet<Products>();
    }
}
