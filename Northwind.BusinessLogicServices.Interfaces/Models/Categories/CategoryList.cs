using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.BusinessLogicServices.Interfaces.Models.Categories
{
    public class CategoryList : Category
    {
        public string CategoryName { get; set; }

        public string Description { get; set; }
    }
}
