using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stize.DotNet.Specification;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;
using Xunit;

namespace Stize.Persistence.Test.Query
{
    public class QueryHandlerFactoryTest
    {
        private readonly ServiceProvider provider;

        public QueryHandlerFactoryTest()
        {
            var services = new ServiceCollection();
            services.AddStizePersistence();
            services.AddSingleton<IMaterializer<Source, Target>, ObjectToObjectMaterializer<Source, Target>>();
            services.AddSingleton<IQueryableProvider, TestQueryableProvider>();
            services.AddLogging();

            this.provider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task MustResolve_SingleResult_With_Query_Source_Source()
        {
            var spec = Specification<Source>.True;
            var query = new Query<Source>(spec);

            var result = await this.MustResolveQueryHandlerAsync<Query<Source>, SingleQueryResult<Source>>(query);
            Assert.NotNull(result.Result);
        }
        
        [Fact]
        public async Task MustResolve_SingleResult_With_Query_Source_Target()
        {
            var spec = Specification<Source>.True;
            var query = new Query<Source>(spec);

            var result = await this.MustResolveQueryHandlerAsync<Query<Source>, SingleQueryResult<Target>>(query);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task MustNotResolve_SingleResult_With_PagedQuery_Source_Target()
        {
            var spec = Specification<Source>.True;
            var query = new PagedQuery<Source>(spec);

            await Assert.ThrowsAsync<System.ArgumentException>(async () =>
            {
                var result = await this.MustResolveQueryHandlerAsync<PagedQuery<Source>, SingleQueryResult<Target>>(query);
                Assert.NotNull(result.Result);
            });
        }

        [Fact]
        public async Task MustResolve_MultipleQueryResult_With_Query_Source_Source()
        {
            var spec = Specification<Source>.True;
            var query = new Query<Source>(spec);

            var result = await this.MustResolveQueryHandlerAsync<Query<Source>, MultipleQueryResult<Source>>(query);
            Assert.NotNull(result.Result);
            Assert.Equal(2,result.Result.Count());
        }

        [Fact]
        public async Task MustResolve_MultipleQueryResult_With_Query_Source_Target()
        {
            var spec = Specification<Source>.True;
            var query = new Query<Source>(spec);

            var result = await this.MustResolveQueryHandlerAsync<Query<Source>, MultipleQueryResult<Target>>(query);
            Assert.NotNull(result.Result);
            Assert.Equal(2,result.Result.Count());
        }

        [Fact]
        public async Task MustNotResolve_MultipleQueryResult_With_PagedQuery_Source_Target()
        {
            var spec = Specification<Source>.True;
            var query = new PagedQuery<Source>(spec);

            await Assert.ThrowsAsync<System.ArgumentException>(async () =>
            {
                var result = await this.MustResolveQueryHandlerAsync<PagedQuery<Source>, MultipleQueryResult<Target>>(query);
                Assert.NotNull(result.Result);
                Assert.Equal(2,result.Result.Count());
            });
        }

        [Fact]
        public async Task MustResolve_PagedQueryResult_With_PagedQuery_Source_Source()
        {
            var spec = Specification<Source>.True;
            var query = new PagedQuery<Source>(spec, 1);

            var result = await this.MustResolveQueryHandlerAsync<PagedQuery<Source>, PagedQueryResult<Source>>(query);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);
        }

        [Fact]
        public async Task MustResolve_PagedQueryResult_With_PagedQuery_Source_Target()
        {
            var spec = Specification<Source>.True;
            var query = new PagedQuery<Source>(spec, 1);

            var result = await this.MustResolveQueryHandlerAsync<PagedQuery<Source>, PagedQueryResult<Target>>(query);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);
        }

        private async Task<TResult> MustResolveQueryHandlerAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQuery
            where TResult : class, IQueryResult, new()
        {
            var factory = this.provider.GetRequiredService<IQueryHandlerFactory>();
            var handler = factory.GetQueryHandler<TQuery, TResult>();
            var result = await handler.HandleAsync(query);
            Assert.NotNull(result);
            return result;
        }

    }

    public class TestQueryableProvider : IQueryableProvider
    {

        public IQueryable<T> GetQueryable<T>() where T : class
        {
            return new[] { Activator.CreateInstance<T>(), Activator.CreateInstance<T>() }.AsQueryable();
        }

        public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class
        {
            return Task.FromResult(queryable.FirstOrDefault());
        }

        public Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class
        {
            return Task.FromResult(queryable.ToArray());
        }

        public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class
        {
            return Task.FromResult(queryable.Count());
        }
    }

    public class ObjectToObjectMaterializer<TSource, TTarget> : IMaterializer<TSource, TTarget>
    {
        public IQueryable<TTarget> Materialize(IQueryable<TSource> queryable)
        {
            return queryable.Select(x => Activator.CreateInstance<TTarget>());
        }
    }

    public class Source
    {
    }

    public class Target
    {
    }
}
