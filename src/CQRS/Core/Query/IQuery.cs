using Stize.CQRS.Mediator;
using Stize.DotNet.Result;

namespace Stize.CQRS.Query
{
    public interface IQuery<TResult> : IRequest<TResult>
        where TResult : IValueResult
    {
    }

    public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
        where TResult : IValueResult
    {
    }
}
