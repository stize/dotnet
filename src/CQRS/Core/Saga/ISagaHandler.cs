using Stize.DotNet.Result;
using Stize.Mediator;

namespace Stize.CQRS.Saga
{
    public interface ISagaHandler<TSaga, TResult> : IRequestHandler<TSaga, TResult>
        where TSaga : ISaga<TResult>
        where TResult : IValueResult
    {       
    }
}
