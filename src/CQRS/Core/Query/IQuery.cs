using Stize.DotNet.Result;
using Stize.Mediator;

namespace Stize.CQRS.Query
{
    public interface IQuery<TResult> : IRequest<TResult>
        where TResult : IValueResult
    {
    }
}
