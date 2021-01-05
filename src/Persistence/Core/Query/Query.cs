using System.Collections.Generic;
using System.Linq;
using Stize.DotNet.Search.Sort;
using Stize.DotNet.Specification;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Query
{
    public abstract class Query<TResult> : IQuery<TResult>
        where TResult : IQueryResult
    {
        
    }

    public abstract class Query<TSource, TTarget, TResult> : Query<TResult>, IQuery<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : IQueryResult<TTarget>
    {

        public IQueryable<TSource> SourceQuery { get; set; }
        public IQueryableProvider Provider { get; set; } = DefaultQueryableProvider.Instance;

        public ISpecification<TSource> SourceSpecification { get; set; }
        public ISpecification<TTarget> TargetSpecification { get; set; }

        public IEnumerable<SortDescriptor> SourceSorts { get; set; }
        public IEnumerable<SortDescriptor> TargetSorts { get; set; }

    }

}