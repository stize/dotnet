using Stize.CQRS.Mediator;

namespace Stize.CQRS.Command
{
    public interface ICommand<TResult> : IRequest<TResult>
        where TResult : ICommandResult
    {
    }

    public interface ICommandResult : IRequestResult
    {
    }

    public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
        where TResult : ICommandResult
    {        
    }

    
}
