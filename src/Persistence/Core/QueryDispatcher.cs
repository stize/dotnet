using System;
using Microsoft.Extensions.DependencyInjection;
using Stize.Persistence.Query;
using Stize.Persistence.QueryHandler;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public TResult Dispatch<TQuery, TSource, TTarget, TResult>(TQuery query)
            where TQuery : IQuery<TSource>
            where TSource : class
            where TTarget : class
            where TResult : IQueryResult
        {
            var handler = this.serviceProvider.GetRequiredService<IQueryHandler<TQuery, TSource, TTarget, TResult>>();
            var result = handler.Handle(query);
            return result;
        }
    }
}