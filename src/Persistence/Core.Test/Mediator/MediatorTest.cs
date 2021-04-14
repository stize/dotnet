using Microsoft.Extensions.DependencyInjection;
using Stize.Mediator;
using Stize.Persistence.Inquiry;
using Stize.Persistence.Materializer;
using Stize.Persistence.Test.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Stize.Persistence.Test.Mediator
{
    public class MediatorTest
    {
        private readonly ServiceProvider provider;

        public MediatorTest()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddStizeMediator();
            services.AddStizePersistence();

            services.AddSingleton(typeof(IMaterializer<,>), typeof(ObjectToObjectMaterializer<,>));
                        
            this.provider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task Handle_SingleValueInquiry_Test()
        {
            using (var scope = this.provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var request = new SingleValueInquiry<Source, Target>()
                {
                    SourceQuery = Array.Empty<Source>().AsQueryable()
                };
                var result = await mediator.HandleAsync(request, default);
                Assert.NotNull(result);                
            }
        }

        [Fact]
        public async Task Handle_MultipleValueInquiry_Test()
        {
            using (var scope = this.provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var request = new MultipleValueInquiry<Source, Target>()
                {
                    SourceQuery = Array.Empty<Source>().AsQueryable()
                };
                var result = await mediator.HandleAsync(request, default);
                Assert.NotNull(result);
            }
        }

        [Fact]
        public async Task Handle_PagedValueInquiry_Test()
        {
            using (var scope = this.provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var request = new PagedValueInquiry<Source, Target>()
                {
                    SourceQuery = Array.Empty<Source>().AsQueryable()
                };
                var result = await mediator.HandleAsync(request, default);
                Assert.NotNull(result);
            }
        }
    }
}
