using Stize.CQRS.Mediator;
using Stize.DotNet.Result;

namespace Stize.CQRS.Command
{
    public interface ICommand<TResult> : IRequest<TResult>
        where TResult : IValueResult
    {
    }

    public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
        where TResult : IValueResult
    {        
    }

    
}
