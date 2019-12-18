using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Northwind.BusinessLogicServices.Interfaces;
using Northwind.BusinessLogicServices.Interfaces.Models;
using Northwind.Common.Extensions;
using Northwind.DataAccess.Interfaces.Context;
using Northwind.DataAccess.Interfaces.Extensions.Quaryable;
using Northwind.DataAccess.Interfaces.Models;

namespace Northwind.BusinessLogicServices
{
    public class SuppliersService : ISuppliersService
    {
        private readonly ISuppliersContext _suppliersContext;
        private readonly IMapper _mapper;

        public SuppliersService(ISuppliersContext suppliersContext, IMapper mapper)
        {
            _suppliersContext = suppliersContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TMapTo>> GetSuppliersAsync<TMapTo>(int? count = null) 
            where TMapTo : Supplier
        {
            var dbSuppliers = await _suppliersContext.Suppliers.Take(count).ToListAsync();

            var mappedProducts = dbSuppliers.Select(p => _mapper.Map<Suppliers, TMapTo>(p)).ToList();

            return mappedProducts;
        }
    }
}
