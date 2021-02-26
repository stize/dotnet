using System.Threading;
using System.Threading.Tasks;

namespace Stize.Persistence.QueryDispatcher
{
    public interface IQueryRequest<TResponse>
    {
    }


    public interface IQueryRequestHandler<in TRequest, TResponse>
        where TRequest : IQueryRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest query, CancellationToken cancellationToken = default);
    }
}