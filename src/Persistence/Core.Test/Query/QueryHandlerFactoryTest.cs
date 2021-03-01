using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryDispatcher;
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
            services.AddSingleton(typeof(IMaterializer<,>), typeof(ObjectToObjectMaterializer<,>));
            services.AddLogging();

            this.provider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task MustResolve_SingleResult_With_Query_Source()
        {
            var query = new SingleValueQuery<Source>();

            var result = await this.MustResolveQueryHandlerAndHandleAsync<SingleValueQuery<Source>, Source, Source, SingleQueryResult<Source>>(query);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task MustResolve_SingleResult_With_Query_Source_Target()
        {
            var query = new SingleValueQuery<Source, Target>();

            var result = await this.MustResolveQueryHandlerAndHandleAsync<SingleValueQuery<Source, Target>, Source, Target, SingleQueryResult<Target>>(query);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task MustResolve_MultipleQueryResult_With_Query_Source()
        {
            var query = new MultipleValueQuery<Source>();

            var result = await this.MustResolveQueryHandlerAndHandleAsync<MultipleValueQuery<Source>, Source, Source, MultipleQueryResult<Source>>(query);
            Assert.NotNull(result.Result);
            Assert.Equal(2, result.Result.Count());
        }

        [Fact]
        public async Task MustResolve_MultipleQueryResult_With_Query_Source_Target()
        {
            var query = new MultipleValueQuery<Source, Target>();

            var result = await this.MustResolveQueryHandlerAndHandleAsync<MultipleValueQuery<Source, Target>, Source, Target, MultipleQueryResult<Target>>(query);
            Assert.NotNull(result.Result);
            Assert.Equal(2, result.Result.Count());
        }


        [Fact]
        public async Task MustResolve_PagedQueryResult_With_PagedQuery_Source()
        {
            var query = new PagedValueQuery<Source>() { Take = 1 };

            var result = await this.MustResolveQueryHandlerAndHandleAsync<PagedValueQuery<Source>, Source, Source, PagedQueryResult<Source>>(query);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);
        }

        [Fact]
        public async Task MustResolve_PagedQueryResult_With_PagedQuery_Source_Target()
        {
            var query = new PagedValueQuery<Source, Target>() { Take = 1 };

            var result = await this.MustResolveQueryHandlerAndHandleAsync<PagedValueQuery<Source, Target>, Source, Target, PagedQueryResult<Target>>(query);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);
        }

        private async Task<TResult> MustResolveQueryHandlerAndHandleAsync<TQuery, TSource, TTarget, TResult>(TQuery query)
            where TQuery : IQuery<TSource, TTarget, TResult>
            where TSource : class
            where TTarget : class
            where TResult : class, IQueryResult<TTarget>
        {

            query.SourceQuery = this.GetQueryable<TSource>();
            
            var factory = this.provider.GetRequiredService<IQueryHandlerFactory>();
            var handler = factory.GetHandler<TQuery, TResult>();
            var result = await handler.HandleAsync(query);
            Assert.NotNull(result);
            return result;
        }

        private IQueryable<T> GetQueryable<T>() where T : class
        {
            return new[] { Activator.CreateInstance<T>(), Activator.CreateInstance<T>() }.AsQueryable();
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
}
