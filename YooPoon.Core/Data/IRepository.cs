using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace YooPoon.Core.Data
{
    public interface IRepository<T> where T : class,IBaseEntity
    {
        T GetById(object id);
        T Get(Expression<Func<T, bool>> expression);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }

        IDbSet<T> DbSet { get; }

        void AddOrUpdate(T[] entity);

        void BulkInsert(IEnumerable<T> entities);
    }
}