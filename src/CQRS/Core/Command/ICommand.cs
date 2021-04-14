using Stize.DotNet.Result;
using Stize.Mediator;

namespace Stize.CQRS.Command
{
    public interface ICommand<TResult> : IRequest<TResult>
        where TResult : IValueResult
    {
    }

    
}
