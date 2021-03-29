using Stize.DotNet.Result;
using Stize.Mediator;

namespace Stize.CQRS.Saga
{
    public interface ISaga<TResult>: IRequest<TResult>
        where TResult : IValueResult
    {
    }
}
