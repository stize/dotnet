using Stize.DotNet.Result;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.Mediator.Test
{
    public class TestRequest : IRequest<Result>
    {
        public TestRequest()
        {
        }
    }

    public class TestRequestHandler : IRequestHandler<TestRequest, Result>
    {
        public Task<Result> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Success());
        }
    }
}