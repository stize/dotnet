using Stize.CQRS.Mediator;
using Stize.DotNet.Result;

namespace Stize.CQRS.Saga
{
    public interface ISaga<TResult>: IRequest<TResult>
        where TResult : IValueResult
    {
    }

    public interface ISagaHandler<TSaga, TResult> : IRequestHandler<TSaga, TResult>
        where TSaga : ISaga<TResult>
        where TResult : IValueResult
    {       
    }
}
