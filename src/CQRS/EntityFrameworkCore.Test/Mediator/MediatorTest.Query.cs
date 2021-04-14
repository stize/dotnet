using Microsoft.Extensions.DependencyInjection;
using Stize.CQRS.EntityFrameworkCore.Query;
using Stize.CQRS.EntityFrameworkCore.Test.Internal;
using Stize.DotNet.Search.Page;
using Stize.Mediator;
using System.Threading.Tasks;
using Xunit;

namespace Stize.CQRS.EntityFrameworkCore.Test.Mediator
{
    public class MediatorQueryTest : MediatorTest
    {

        [Fact]
        public async Task Handle_GetModelFromEntityByIdQuery_Test()
        {
            using (var scope = this.provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var id = 1;
                var request = new GetModelFromEntityByIdQuery<Model, Entity, int, EntityDbContext>(id);
                var result = await mediator.HandleAsync(request, default);
                Assert.NotNull(result);
            }
        }

        [Fact]
        public async Task Handle_GetAllModelsFromEntityQuery_Test()
        {
            using (var scope = this.provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var request = new GetAllModelsFromEntityQuery<Model, Entity, int, EntityDbContext>();
                var result = await mediator.HandleAsync(request, default);
                Assert.NotNull(result);
            }
        }

        [Fact]
        public async Task Handle_GetAllModelsFromEntityByPageQuery_Test()
        {
            using (var scope = this.provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var pageDescriptor = new PageDescriptor()
                {
                    Take = 1
                };
                var request = new GetAllModelsFromEntityByPageQuery<Model, Entity, int, EntityDbContext>(pageDescriptor);
                var result = await mediator.HandleAsync(request, default);
                Assert.NotNull(result);
            }
        }
    }
}
