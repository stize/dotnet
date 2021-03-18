using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stize.Persistence.Materializer;
using Stize.Persistence.Inquiry;
using Stize.Persistence.InquiryDispatcher;
using Xunit;
using Stize.DotNet.Result;

namespace Stize.Persistence.Test.Inquiry
{
    public class InquiryHandlerFactoryTest
    {
        private readonly ServiceProvider provider;

        public InquiryHandlerFactoryTest()
        {
            var services = new ServiceCollection();
            services.AddStizePersistence();
            services.AddSingleton(typeof(IMaterializer<,>), typeof(ObjectToObjectMaterializer<,>));
            services.AddLogging();

            this.provider = services.BuildServiceProvider();
        }


        [Fact]
        public async Task MustResolve_SingleResult()
        {
            var Inquiry = new SingleValueInquiry<Source, Target>();

            var result = await this.MustResolveInquiryHandlerAndHandleAsync<SingleValueInquiry<Source, Target>, Source, Target, SingleValueResult<Target>>(Inquiry);
            Assert.NotNull(result.Value);
        }


        [Fact]
        public async Task MustResolve_MultipleInquiryResult()
        {
            var Inquiry = new MultipleValueInquiry<Source, Target>();

            var result = await this.MustResolveInquiryHandlerAndHandleAsync<MultipleValueInquiry<Source, Target>, Source, Target, MultipleValueResult<Target>>(Inquiry);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count());
        }


        [Fact]
        public async Task MustResolve_PagedInquiryResult()
        {
            var Inquiry = new PagedValueInquiry<Source, Target>() { Take = 1 };

            var result = await this.MustResolveInquiryHandlerAndHandleAsync<PagedValueInquiry<Source, Target>, Source, Target, PagedValueResult<Target>>(Inquiry);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value);
        }

        private async Task<TResult> MustResolveInquiryHandlerAndHandleAsync<TInquiry, TSource, TTarget, TResult>(TInquiry Inquiry)
            where TInquiry : IInquiry<TSource, TTarget, TResult>
            where TSource : class
            where TTarget : class
            where TResult : class, IValueResult<TTarget>
        {

            Inquiry.SourceQuery = this.GetInquiryable<TSource>();
            
            var factory = this.provider.GetRequiredService<IInquiryHandlerFactory>();
            var handler = factory.GetHandler<TInquiry, TResult>();
            var result = await handler.HandleAsync(Inquiry);
            Assert.NotNull(result);
            return result;
        }

        private IQueryable<T> GetInquiryable<T>() where T : class
        {
            return new[] { Activator.CreateInstance<T>(), Activator.CreateInstance<T>() }.AsQueryable();
        }


        public class ObjectToObjectMaterializer<TSource, TTarget> : IMaterializer<TSource, TTarget>
        {
            public IQueryable<TTarget> Materialize(IQueryable<TSource> Inquiryable)
            {
                return Inquiryable.Select(x => Activator.CreateInstance<TTarget>());
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
