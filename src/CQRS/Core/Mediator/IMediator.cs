using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.Mediator
{

    public interface IRequest<TResult> 
        where TResult : IRequestResult
    {
    }

    public interface IRequestResult
    {

    }

    public interface IRequestHandler<TRequest, TResult>
        where TRequest : IRequest<TResult>
        where TResult : IRequestResult
    {
        public Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }

    public interface IMediator
    {
        public Task<TResult> SendAsync<TResult>(IRequest<TResult> request)
            where TResult : IRequestResult;
    }
}
