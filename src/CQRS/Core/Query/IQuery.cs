using Stize.CQRS.Mediator;

namespace Stize.CQRS.Query
{
    public interface IQuery<TResult> : IRequest<TResult>
        where TResult : IQueryResult
    {
    }

    public interface IQueryResult : IRequestResult
    {
    }

    public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
        where TResult : IQueryResult
    {
    }
}
