using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface IMultipleValueQueryHandler<TQuery, TSource, TTarget> : IQueryHandler<TQuery, TSource, TTarget, IMultipleQueryResult<TTarget>>
        where TQuery : IMultipleValueQuery<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
    }

    public class MultipleValueQueryHandler<TQuery, TSource, TTarget> : QueryHandler<TQuery, TSource, TTarget, IMultipleQueryResult<TTarget>>, IMultipleValueQueryHandler<TQuery, TSource, TTarget>
       where TQuery : IMultipleValueQuery<TSource, TTarget>
       where TSource : class
       where TTarget : class
    {
        public MultipleValueQueryHandler(IEntityMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }

        protected override IMultipleQueryResult<TTarget> GenerateResult(IQueryable<TTarget> queryable)
        {            
            var values = queryable.ToArray();
            return new MultipleQueryResult<TTarget>(values);
        }                
    }
}
