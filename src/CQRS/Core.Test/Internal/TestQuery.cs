using Stize.CQRS.Query;
using Stize.DotNet.Result;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.Test.Internal
{
    public class TestQuery : IQuery<Result>
    {
    }

    public class TestQueryHandler : IQueryHandler<TestQuery, Result>
    {
        public Task<Result> HandleAsync(TestQuery request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Success());
        }
    }
}
