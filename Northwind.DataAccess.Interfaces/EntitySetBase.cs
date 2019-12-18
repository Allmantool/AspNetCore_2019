using Microsoft.EntityFrameworkCore.Query;
using Northwind.DataAccess.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.DataAccess.Interfaces
{
    public abstract class EntitySetBase<TEntity> : IQueryable<TEntity>, IEntitySet<TEntity>
       where TEntity : class
    {
        public Expression Expression => Queryable.Expression;

        public Type ElementType => Queryable.ElementType;

        public IQueryProvider Provider => Queryable.Provider;

        protected abstract IQueryable<TEntity> Queryable { get; }

        public IEnumerator<TEntity> GetEnumerator() => Queryable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => (Queryable as IEnumerable).GetEnumerator();

        public abstract TEntity Add(TEntity entity);

        public abstract Task<TEntity> AddAsync(TEntity entityToAdd);

        public abstract TEntity Remove(TEntity entity);

        public abstract TEntity Update(TEntity entity);

    }
}
