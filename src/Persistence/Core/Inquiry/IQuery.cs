using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.DotNet.Search.Sort;
using Stize.DotNet.Specification;
using Stize.Persistence.QueryDispatcher;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Query
{
    public interface IQuery<TResult> : IQueryRequest<TResult>
        where TResult : IQueryResult
    {
       
    }

    public interface IQuery<TSource, TTarget, TResult> : IQuery<TResult>
        where TSource : class
        where TTarget : class
        where TResult : IQueryResult<TTarget>
    {
        
        public IQueryableProvider Provider { get; set; }
        public IQueryable<TSource> SourceQuery { get; set; }

        ISpecification<TSource> SourceSpecification { get; }
        ISpecification<TTarget> TargetSpecification { get; }

        public IEnumerable<SortDescriptor> SourceSorts { get; }
        public IEnumerable<SortDescriptor> TargetSorts { get; }

        
    }



}