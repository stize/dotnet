using Microsoft.Extensions.DependencyInjection;
using Stize.CQRS.Test.Internal;
using Stize.DotNet.Result;
using Stize.Mediator;
using System.Threading.Tasks;
using Xunit;

namespace Stize.CQRS.Test.Mediator
{
    public class MediatorTest
    {
        private readonly ServiceProvider provider;

        public MediatorTest()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddStizeMediator();

            services.AddStizeQueryHandler<TestQueryHandler, TestQuery, Result>();
            services.AddStizeCommandHandler<TestCommandHandler, TestCommand, Result>();
            services.AddStizeSagaHandler<TestSagaHandler, TestSaga, Result>();

            provider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task Handle_Query_Test()
        {
            using (var scope = provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var query = new TestQuery();
                var result = await mediator.HandleAsync(query, default);
                Assert.NotNull(result);
            }
        }

        [Fact]
        public async Task Handle_Command_Test()
        {
            using (var scope = provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var query = new TestCommand();
                var result = await mediator.HandleAsync(query, default);
                Assert.NotNull(result);
            }
        }


        [Fact]
        public async Task Handle_Saga_Test()
        {
            using (var scope = provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var query = new TestSaga();
                var result = await mediator.HandleAsync(query, default);
                Assert.NotNull(result);
            }
        }
    }
}
