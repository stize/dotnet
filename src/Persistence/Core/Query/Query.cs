using Stize.DotNet.Specification;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Query
{
    public abstract class Query<TEntity, TResult> : IQuery<TEntity, TResult> 
        where TEntity : class 
        where TResult : IQueryResult
    {
        public ISpecification<TEntity> Specification { get; }

        protected Query(ISpecification<TEntity> specification)
        {
            this.Specification = specification;
        }
    }
}