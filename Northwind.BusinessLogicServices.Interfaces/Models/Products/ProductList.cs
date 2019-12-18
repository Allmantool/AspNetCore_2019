using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.BusinessLogicServices.Interfaces.Models.Products
{
    public class ProductList : Product
    {
        public string SupplierName { get; set; }

        public string CategoryName { get; set; }

    }
}
