using Stize.CQRS.Mediator;

namespace Stize.CQRS.Saga
{
    public interface ISaga<TResult>: IRequest<TResult>
        where TResult : ISagaResult
    {
    }

    public interface ISagaResult : IRequestResult
    {

    }

    public interface ISagaHandler<TSaga, TResult> : IRequestHandler<TSaga, TResult>
        where TSaga : ISaga<TResult>
        where TResult : ISagaResult
    {       
    }
}
