using System;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryDispatcher
{
    internal abstract class QueryRequestHandlerWrapper<TResult>
        where TResult : IQueryResult
    {
        public abstract Task<TResult> HandleAsync(IQuery<TResult> query, IQueryHandlerFactory handlerFactory, CancellationToken cancellationToken = default);
    }

    internal class QueryRequestHandlerWrapper<TQuery, TResult> : QueryRequestHandlerWrapper<TResult>
        where TQuery : class, IQuery<TResult>
        where TResult : IQueryResult
    {

        public override async Task<TResult> HandleAsync(IQuery<TResult> query, IQueryHandlerFactory handlerFactory, CancellationToken cancellationToken = default)
        {
            var q = query as TQuery;

            var handler = GetHandler(handlerFactory);
            var result = await handler.HandleAsync(q, cancellationToken);
            return result;
        }

        public IQueryRequestHandler<TQuery, TResult> GetHandler(IQueryHandlerFactory handlerFactory)
        {
            var handler = handlerFactory.GetHandler<TQuery, TResult>();
            if (handler == null)
            {
                throw new ArgumentException($"Cant find a handler for query {typeof(TQuery).FullName}");
            }

            return handler;
        }
    }
}