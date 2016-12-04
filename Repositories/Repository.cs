using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AnalyseVisitorsTool.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AnalyseVisitorsTool.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext context;

        public Repository(DbContext context)
        {
            this.context = context;
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this.context.Set<TEntity>().Where(predicate).First();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.context.Set<TEntity>().Where(predicate);
        }

        public void Add(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            if (entities.Count() > 10) {
                var page = 0;
                var maxpages = Math.Round(Convert.ToDecimal(entities.Count() / 10), 0) + 1;
                do {
                    this.context.Set<TEntity>().AddRange(entities.Skip(page * 10).Take(10));
                    this.Save();
                    page++;
                } while (maxpages > page);
            } else {
                this.context.Set<TEntity>().AddRange(entities);
            }
        }

        public void Remove(TEntity entity)
        {
            this.context.Set<TEntity>().Remove(entity);
        }

        public void Save()
        {
            this.context.SaveChangesAsync();
        }
    }
}
