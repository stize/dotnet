using Microsoft.Extensions.DependencyInjection;
using Stize.CQRS.EntityFrameworkCore.Command;
using Stize.CQRS.EntityFrameworkCore.Test.Internal;
using Stize.DotNet.Delta;
using Stize.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stize.CQRS.EntityFrameworkCore.Test.Mediator
{
    public partial class MediatorCommandTest : MediatorTest
    {

        [Fact]
        public async Task Handle_CreateEntityFromModelCommand_Test()
        {
            using (var scope = this.provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var model = new Model();
                var request = new CreateEntityFromModelCommand<Model, Entity, int, EntityDbContext>(model);
                var result = await mediator.HandleAsync(request, default);
                Assert.NotNull(result);
            }
        }


        [Fact]
        public async Task Handle_UpdateEntityFromModelCommand_Test()
        {
            using (var scope = this.provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var model = new Model();
                var request = new UpdateEntityFromModelCommand<Model, Entity, int, EntityDbContext>(model);
                var result = await mediator.HandleAsync(request, default);
                Assert.NotNull(result);
            }
        }


        [Fact]
        public async Task Handle_PatchEntityFromModelCommand_Test()
        {
            using (var scope = this.provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var delta = new Delta<Model>();
                var request = new PatchEntityFromModelCommand<Model, Entity, int, EntityDbContext>(delta);
                var result = await mediator.HandleAsync(request, default);
                Assert.NotNull(result);
            }
        }


        [Fact]
        public async Task Handle_DeleteEntityByIdCommand_Test()
        {
            using (var scope = this.provider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var id = 1;
                var request = new DeleteEntityByIdCommand<Entity, int, EntityDbContext>(id);
                var result = await mediator.HandleAsync(request, default);
                Assert.NotNull(result);
            }
        }

    }
}
