using Northwind.BusinessLogicServices.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.BusinessLogicServices.Models
{
    public interface IProductsService
    {
        public Task<IEnumerable<ProductList>> GetAllProductsAsync();
    }
}
