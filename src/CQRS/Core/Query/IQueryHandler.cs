using Stize.DotNet.Result;
using Stize.Mediator;

namespace Stize.CQRS.Query
{
    public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
        where TResult : IValueResult
    {
    }
}
