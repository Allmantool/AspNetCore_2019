using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CoreExtensions = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

namespace Northwind.DataAccess.Interfaces.Extensions.Quaryable
{
    public static class QuaryableExtensions
    {
       public static Task<List<T>> ToListAsync<T>(this IQueryable<T> source)
       where T : class
        {
            if (source is EntitySet<T>)
            {
                source = (source as EntitySet<T>).DbSet;
            }
            
            return CoreExtensions.ToListAsync(source);
        }

        public static Task<T[]> ToArrayAsync<T>(this IQueryable<T> source)
            where T : class
        {
            if (source is EntitySet<T>)
            {
                source = (source as EntitySet<T>).DbSet;
            }

            return CoreExtensions.ToArrayAsync(source);
        }

        public static IQueryable<TEntity> Include<TEntity, TRelatedEntityProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TRelatedEntityProperty>> expression)
            where TEntity : class
            where TRelatedEntityProperty : class
        {
            if(source is EntitySet<TEntity>)
            {
                source = (source as EntitySet<TEntity>).DbSet;
            }

            return CoreExtensions.Include<TEntity, TRelatedEntityProperty>(source, expression);
        }

        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> source, string navigationPropertyPath)
            where TEntity : class
        {
            if (source is EntitySet<TEntity>)
            {
                source = (source as EntitySet<TEntity>).DbSet;
            }

            return CoreExtensions.Include<TEntity>(source, navigationPropertyPath);
        }
    }
}
