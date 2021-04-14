using Microsoft.Extensions.DependencyInjection;
using Stize.DotNet.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stize.Mediator.Test
{
    public class MediatorTest
    {
        private readonly ServiceProvider provider;

        public MediatorTest()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddStizeMediator();
            services.AddStizeMediatorHandler<TestRequestHandler>();
            this.provider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task MediatorMustHandleWhenResolvedFromServiceProvider()
        {
            using (var scope = this.provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var request = new TestRequest();
                var result = await mediator.HandleAsync(request, default);
            }
        }
    }
}
