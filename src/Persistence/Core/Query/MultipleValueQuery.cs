using Stize.DotNet.Specification;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Query
{
    public class MultipleValueQuery<TEntity, TResult> : Query<TEntity, IMultipleQueryResult<TResult>>, IMultipleValueQuery<TEntity, TResult> 
        where TEntity : class 
        where TResult : class
    {
        public MultipleValueQuery(ISpecification<TEntity> specification) : base(specification)
        {
        }
    }
}