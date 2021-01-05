using System;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Mediator
{
    public class Mediator : IMediator
    {
        private readonly IRequestHandlerFactory handlerFactory;

        public Mediator(IRequestHandlerFactory handlerFactory)
        {
            this.handlerFactory = handlerFactory;
        }

        public async Task<TResult> HandleAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
            where TResult : class, IQueryResult
        {

            var wrapper = (RequestHandlerWrapper<TResult>)Activator.CreateInstance(typeof(RequestHandlerWrapper<,>).MakeGenericType(query.GetType(), typeof(TResult)));
            var result = await wrapper.HandleAsync(query, this.handlerFactory, cancellationToken);
            return result;
        }


    }


    
}