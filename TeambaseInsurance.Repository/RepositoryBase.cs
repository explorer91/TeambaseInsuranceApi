using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TeambaseInsurance.RepositoryContracts;

namespace TeambaseInsurance.Repository
{
    abstract public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext RepositoryContext;
        
        protected RepositoryBase(RepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        
        public void Create(T entity)
        {
            RepositoryContext.Set<T>().Add(entity);
        }

        public void CreateRange(IEnumerable<T> entities)
        {
            RepositoryContext.Set<T>().AddRange(entities);
        }

        public void Delete(T entity)
        {
            RepositoryContext.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            RepositoryContext.Set<T>().RemoveRange(entities);
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return trackChanges ?
                RepositoryContext.Set<T>() :
                RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return trackChanges ?
                RepositoryContext.Set<T>().Where(expression) :
                RepositoryContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
        }
        
        public void UpdateRange(IEnumerable<T> entities)
        {
            RepositoryContext.Set<T>().UpdateRange(entities);
        }
    }
}
