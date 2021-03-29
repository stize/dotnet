using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stize.CQRS.EntityFrameworkCore.Test.Internal;
using Stize.DotNet.Search.Specification;
using Stize.Persistence.Materializer;
using System;

namespace Stize.CQRS.EntityFrameworkCore.Test.Mediator
{
    public class MediatorTest
    {
        protected readonly ServiceProvider provider;

        public MediatorTest()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddStizeMediator();
            services.AddStizePersistence();
            services.AddStizeEntityDbContext<EntityDbContext>(builder =>
            {
                builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
            services.AddStizeEntityRepository();
            services.AddStizeCqrsEntityFrameworkCore();
            services.AddScoped<ISpecificationBuilder, SpecificationBuilder>();

            services.AddScoped(typeof(IMaterializer<,>), typeof(ObjectToObjectMaterializer<,>));
            services.AddScoped(typeof(IEntityBuilder<,,>), typeof(ObjectToObjectBuilder<,,>));

            this.provider = services.BuildServiceProvider();
            this.EnsureDatabaseCreated();
        }

        private void EnsureDatabaseCreated()
        {
            using (var scope = this.provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DbContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }
}
