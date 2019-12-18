using Northwind.DataAccess.Interfaces;
using Northwind.DataAccess.Interfaces.Context;
using Northwind.DataAccess.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.DataAccess.Context
{
    public class CategoriesContext : AppDomainContextBase<NorthwindContext>, ICategoriesContext
    {
        public CategoriesContext(NorthwindContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<Categories> Categories => base.GetEntitySet<Categories>();
    }
}
