using Stize.CQRS.Saga;
using Stize.DotNet.Result;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.Test.Internal
{
    public class TestSaga : ISaga<Result>
    {
    }

    public class TestSagaHandler : ISagaHandler<TestSaga, Result>
    {
        public Task<Result> HandleAsync(TestSaga request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Success());
        }
    }
}
