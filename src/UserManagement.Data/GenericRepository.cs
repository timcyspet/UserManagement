using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Data.Common;
using UserManagement.Model;

namespace UserManagement.Data
{
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Get all entities from db
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        List<TEntity> Get(bool IsActive = true,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get query for entity
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IQueryable<TEntity> Query(bool IsActive = true, Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Get single entity by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(int id, bool IsActive = true, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get first or default entity by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        TEntity GetFirstOrDefault(bool IsActive = true,
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Insert entity to db
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// Update entity in db
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);


        /// <summary>
        /// Attach entity in db
        /// </summary>
        /// <param name="entity"></param>
        //void Attach(TEntity entity);

        /// <summary>
        /// Delete entity from db by primary key
        /// </summary>
        /// <param name="id"></param>
        void Delete(int Id);

        /// <summary>
        /// Mark an entity inactive in db by primary key
        /// </summary>
        /// <param name="id"></param>
        void Active(int id, bool IsActive = true);

        /// <summary>
        /// save all DB Changes
        /// </summary>
        /// <param ></param>
        void Save();
    }
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseObject
    {
        protected DbContext context;
        protected DbSet<TEntity> dbSet;
        protected User user;
        private ILogger logger;

        public GenericRepository(DbContext context, ILogger logger,User user)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
            this.logger = logger;
            this.user = user;

        }

        public virtual List<TEntity> Get(bool IsActive = true, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);

                if (filter != null)
                    query = query.Where(filter);

                query = query.Where(item => item.IsDeleted == false && (item.IsActive == IsActive || IsActive == false));

                if (orderBy != null)
                    query = orderBy(query);

                return query.ToList();
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
                throw;
            }
        }

        public virtual IQueryable<TEntity> Query(bool IsActive = true, Expression<Func<TEntity, bool>> filter = null)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if (filter != null)
                    query = query.Where(filter);

                query = query.Where(item => item.IsDeleted == false && (item.IsActive == IsActive || IsActive == false));

                return query;
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
                throw;
            }
        }

        public virtual TEntity GetById(int id, bool IsActive = true, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);

                return query.Where(item => item.IsDeleted == false && (item.IsActive == IsActive || IsActive == false) && item.Id == id).FirstOrDefault();
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
                throw;
            }
        }

        public virtual TEntity GetFirstOrDefault(bool IsActive = true, Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);

                query = query.Where(item => item.IsDeleted == false && (item.IsActive == IsActive || IsActive == false));

                return query.FirstOrDefault(filter);
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
                throw;
            }
        }

        public virtual void Insert(TEntity entity)
        {
            try
            {
                entity.CreatedBy = user.email;

                entity.CreatedOn = DateTime.Now;

                entity.UpdatedBy = user.email;

                entity.UpdatedOn = DateTime.Now;

                entity.IsDeleted = false;

                entity.IsActive = true;

                dbSet.Add(entity);

                //this.context.SaveChanges();


            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
                throw;
            }
        }

        public virtual void Update(TEntity entity)
        {
            try
            {
                entity.UpdatedBy = user.email;

                entity.UpdatedOn = DateTime.Now;

                dbSet.Attach(entity);

                context.Entry(entity).State = EntityState.Modified;

                //this.context.SaveChanges();
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
                throw;
            }
        }

        public virtual void Active(int id, bool IsActive = true)
        {
            try
            {
                TEntity entityToDelete = dbSet.Find(id);

                entityToDelete.UpdatedBy = user.email;

                entityToDelete.UpdatedOn = DateTime.Now;

                entityToDelete.IsActive = IsActive;

                if (context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }
                context.Entry(entityToDelete).State = EntityState.Modified;

                // this.context.SaveChanges();
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
                throw;
            }

        }

        public virtual void Delete(int Id)
        {
            try
            {
                TEntity entityToDelete = dbSet.Find(Id);

                entityToDelete.UpdatedBy = user.email;

                entityToDelete.UpdatedOn = DateTime.Now;

                entityToDelete.IsDeleted = true;

                entityToDelete.IsActive = false;

                if (context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }
                context.Entry(entityToDelete).State = EntityState.Modified;

                //this.context.SaveChanges();

                //dbSet.Remove(entityToDelete);
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
                throw;
            }
        }

        public virtual void Save()
        {
            try
            {
                this.context.SaveChanges();
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
                throw;
            }

        }

    }
}
