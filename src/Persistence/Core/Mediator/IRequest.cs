using System.Threading;
using System.Threading.Tasks;

namespace Stize.Persistence.Mediator
{
    public interface IRequest<TResponse>
    {
    }

    
    public interface IRequestHandler<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest query, CancellationToken cancellationToken = default);
    }
}