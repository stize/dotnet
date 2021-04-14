using Stize.DotNet.Result;
using Stize.Mediator.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.Mediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider provider;

        public Mediator(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public Task<TResult> HandleAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default) where TResult : IValueResult
        {

            var wrapper = (RequestWrapper<TResult>)Activator.CreateInstance(typeof(RequestWrapper<,>).MakeGenericType(request.GetType(), typeof(TResult)));
            var result = wrapper.HandleAsync(this.provider, request, cancellationToken);
            return result;
        }
    }
}
