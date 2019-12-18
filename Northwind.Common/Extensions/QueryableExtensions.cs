using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Take<TEntity>(this IQueryable<TEntity> source, int? amountToTake)
        {
            if (!amountToTake.HasValue)
            {
                return source;
            }

            return Queryable.Take(source, amountToTake.Value);
        }
    }
}
