using System;
using System.Net.Cache;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IQueryHandlerFactory queryHandlerFactory;

        public QueryDispatcher(IQueryHandlerFactory queryHandlerFactory)
        {
            this.queryHandlerFactory = queryHandlerFactory;
        }

        public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : IQuery
            where TResult : class, IQueryResult, new()
        {

            var handler = this.queryHandlerFactory.GetQueryHandler<TQuery, TResult>();
            if (handler == null)
            {
                throw new ArgumentException($"Cant find a handler for query {typeof(TQuery)} and result {typeof(TResult)}");
            }

            return await handler.HandleAsync(query, cancellationToken);
        }
    }
}