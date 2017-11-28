using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Repository.EFEntity.BlogContext;
using EF.Repository.EFEntity.BlogPostContext;

namespace EF.Repository.Context
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal BloggingEntities blogContext;
        internal BlogPostEntities blogPostContext;
        internal DbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(string contextName, string connectionString)//MVCEntities context)
        {
            if (contextName == "Blog")
            {
                context = new BloggingEntities(connectionString);               
                this.dbSet = blogContext.Set<TEntity>();
            }
            if (contextName == "BlogPost")
            {
                context = new BlogPostEntities(connectionString);               
                this.dbSet = blogPostContext.Set<TEntity>();
            }
        }

        public virtual IEnumerable<TEntity> Get()
        {
            IQueryable<TEntity> query = dbSet;
            return query.ToList();
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}