using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using YooPoon.Core.Data;

namespace YooPoon.Data.EntityFramework
{
    /// <summary>
    /// Entity Framework仓库
    /// </summary>
    public class EfRepository<T> : IRepository<T> where T : class,IBaseEntity
    {
        private readonly IDbContext _context;
        private IDbSet<T> _entities;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(IDbContext context)
        {
            _context = context;
        }

        public virtual T GetById(object id)
        {
            return Entities.Find(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return Entities.FirstOrDefault(expression);
        }

        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Add(entity);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors.Aggregate(string.Empty, (current1, validationErrors) => validationErrors.ValidationErrors.Aggregate(current1, (current, validationError) => current + (string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine)));

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors.Aggregate(string.Empty, (current1, validationErrors) => validationErrors.ValidationErrors.Aggregate(current1, (current, validationError) => current + (Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage))));

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                Entities.Remove(entity);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors.Aggregate(string.Empty, (current1, validationErrors) => validationErrors.ValidationErrors.Aggregate(current1, (current, validationError) => current + (Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage))));

                var fail = new Exception(msg, dbEx);

                throw fail;
            }
        }

        IQueryable<T> IRepository<T>.Table
        {
            get { return Table; }
        }

        public IDbSet<T> DbSet
        {
            get { return Entities; }
        }

        public void AddOrUpdate(T[] entity)
        {
            try
            {
                if(entity==null || !entity.Any())
                    return;
                _entities.AddOrUpdate(entity);

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors.Aggregate(string.Empty, (current1, validationErrors) => validationErrors.ValidationErrors.Aggregate(current1, (current, validationError) => current + (Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage))));

                var fail = new Exception(msg, dbEx);

                throw fail;
            }
        }

        public void BulkInsert(IEnumerable<T> entities)
        {
            var efDbContext = ((EfDbContext) _context);
            try
            {
                efDbContext.Configuration.AutoDetectChangesEnabled = false;
                efDbContext.Configuration.ValidateOnSaveEnabled = false;
                var entitySet = efDbContext.Set<T>();
                foreach (var entity in entities)
                {
                    entitySet.Add(entity);
                }
                efDbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = dbEx.EntityValidationErrors.Aggregate(string.Empty,
                    (current1, validationErrors) =>
                        validationErrors.ValidationErrors.Aggregate(current1,
                            (current, validationError) =>
                                current +
                                (Environment.NewLine +
                                 string.Format("Property: {0} Error: {1}", validationError.PropertyName,
                                     validationError.ErrorMessage))));

                var fail = new Exception(msg, dbEx);

                throw fail;
            }
            finally
            {
                efDbContext.Configuration.AutoDetectChangesEnabled = true;
                efDbContext.Configuration.ValidateOnSaveEnabled = true;
            }
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }

        protected virtual IDbSet<T> Entities
        {
            get { return _entities ?? (_entities = _context.Set<T>()); }
        }
    }
}