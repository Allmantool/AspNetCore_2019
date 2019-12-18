using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.BusinessLogicServices.Interfaces.Models.Categories
{
    public class CategoryUpdate : CategoryList
    {
        public byte[] Picture { get; set; }
    }
}
