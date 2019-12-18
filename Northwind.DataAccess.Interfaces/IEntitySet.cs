using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.DataAccess.Interfaces
{
    public interface IEntitySet<TEntity> : IQueryable<TEntity>
      where TEntity : class
    {
        TEntity Add(TEntity entity);

        Task<TEntity> AddAsync(TEntity entityToAdd);

        TEntity Remove(TEntity entity);

        TEntity Update(TEntity entity);

    }
}
