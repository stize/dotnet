using Stize.CQRS.Command;
using Stize.DotNet.Result;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.Test.Internal
{
    public class TestCommand : ICommand<Result>
    {
    }

    public class TestCommandHandler : ICommandHandler<TestCommand, Result>
    {

        public Task<Result> HandleAsync(TestCommand request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Success());
        }
    }

}
