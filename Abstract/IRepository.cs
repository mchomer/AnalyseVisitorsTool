using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AnalyseVisitorsTool.Abstract
{
    public interface IRepository<TEntity> where TEntity : class 
    {
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void Save();
    }
}
