using System;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryDispatcher
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IQueryHandlerFactory handlerFactory;

        public QueryDispatcher(IQueryHandlerFactory handlerFactory)
        {
            this.handlerFactory = handlerFactory;
        }

        public async Task<TResult> HandleAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
            where TResult : class, IQueryResult
        {

            var wrapper = (QueryRequestHandlerWrapper<TResult>)Activator.CreateInstance(typeof(QueryRequestHandlerWrapper<,>).MakeGenericType(query.GetType(), typeof(TResult)));
            var result = await wrapper.HandleAsync(query, handlerFactory, cancellationToken);
            return result;
        }


    }



}