using Stize.DotNet.Result;

namespace Stize.Mediator
{
    public interface IRequest<TResult> 
        where TResult : IValueResult
    {
    }
}
