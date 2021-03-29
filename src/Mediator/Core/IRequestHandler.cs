using Stize.DotNet.Result;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.Mediator
{
    public interface IRequestHandler<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TResult : IValueResult
    {
        public Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
