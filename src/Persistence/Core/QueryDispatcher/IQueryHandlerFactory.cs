namespace Stize.Persistence.QueryDispatcher
{
    public interface IQueryHandlerFactory
    {
        IQueryRequestHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>()
            where TRequest : IQueryRequest<TResponse>;
    }
}