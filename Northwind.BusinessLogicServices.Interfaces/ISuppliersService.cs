using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Northwind.BusinessLogicServices.Interfaces.Models;

namespace Northwind.BusinessLogicServices.Interfaces
{
    public interface ISuppliersService
    {
        Task<IEnumerable<TMapTo>> GetSuppliersAsync<TMapTo>(int? count = null)
            where TMapTo : Supplier;
    }
}
