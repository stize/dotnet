using Stize.DotNet.Result;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.Mediator.Internal
{
    internal class RequestHandlerWrapper<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TResult : IValueResult
    {
        public async Task<TResult> HandleAsync(IRequestHandler<TRequest, TResult> handler, TRequest request, CancellationToken cancellationToken = default)
        {
            var result = await handler.HandleAsync(request, cancellationToken);
            return result;
        }
    }
}
