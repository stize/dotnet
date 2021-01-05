using System;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Mediator
{
    internal abstract class RequestHandlerWrapper<TResult>
        where TResult : IQueryResult
    {
        public abstract Task<TResult> HandleAsync(IQuery<TResult> query, IRequestHandlerFactory handlerFactory, CancellationToken cancellationToken = default);
    }

    internal class RequestHandlerWrapper<TQuery, TResult> : RequestHandlerWrapper<TResult>
        where TQuery : class, IQuery<TResult>
        where TResult : IQueryResult
    {

        public override async Task<TResult> HandleAsync(IQuery<TResult> query, IRequestHandlerFactory handlerFactory, CancellationToken cancellationToken = default)
        {
            var q = query as TQuery;

            var handler = this.GetHandler(handlerFactory);
            var result = await handler.HandleAsync(q, cancellationToken);
            return result;
        }
        
        public IRequestHandler<TQuery, TResult> GetHandler(IRequestHandlerFactory handlerFactory)
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