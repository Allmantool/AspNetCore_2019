using Northwind.DataAccess.Interfaces;
using Northwind.DataAccess.Interfaces.Context;
using Northwind.DataAccess.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.DataAccess.Context
{
    public class SuppliersContext : AppDomainContextBase<NorthwindContext>, ISuppliersContext
    {
        public SuppliersContext(NorthwindContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<Suppliers> Suppliers => base.GetEntitySet<Suppliers>();
    }
}
