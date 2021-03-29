using Stize.DotNet.Result;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.Mediator
{
    public interface IMediator
    {
        public Task<TResult> HandleAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default)
            where TResult : IValueResult;
    }
}
