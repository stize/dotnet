using Stize.DotNet.Result;
using Stize.Mediator;

namespace Stize.CQRS.Command
{
    public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
        where TResult : IValueResult
    {        
    }

    
}
