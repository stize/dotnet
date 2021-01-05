namespace Stize.Persistence.Mediator
{
    public interface IRequestHandlerFactory
    {
        IRequestHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>()
            where TRequest : IRequest<TResponse>;
    }
}