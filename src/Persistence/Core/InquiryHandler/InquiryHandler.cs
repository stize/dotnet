using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Inquiry;
using Stize.DotNet.Result;
using Stize.Persistence.Internal;

namespace Stize.Persistence.InquiryHandler
{
    public abstract class InquiryHandler<TInquiry, TSource, TTarget, TResult> : IInquiryHandler<TInquiry, TSource, TTarget, TResult>
        where TInquiry : IInquiry<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : IValueResult<TTarget>
    {
        protected InquiryHandler(IMaterializer<TSource, TTarget> materializer)
        {
            this.Materializer = materializer;
        }

        protected IMaterializer<TSource, TTarget> Materializer { get; }

        protected TInquiry Inquiry { get; set; }

        public virtual async Task<TResult> HandleAsync(TInquiry inquiry, CancellationToken cancellationToken = default)
        {
            var queryable = await this.ExecuteQueryAsync(inquiry, cancellationToken);

            var result = await this.GenerateResultAsync(queryable, cancellationToken);
            return result;
        }

        protected virtual async Task<IQueryable<TTarget>> ExecuteQueryAsync(TInquiry inquiry, CancellationToken cancellationToken = default)
        {
            this.Inquiry = inquiry;
            var queryable = await this.GetSourceQueryAsync(cancellationToken);

            var sorted = await this.SortAsync(queryable, cancellationToken);
            var filtered = await this.FilterAsync(sorted, cancellationToken);

            var materialized = await this.MaterializeAsync(filtered, cancellationToken);

            var sortedMaterialized = await this.SortAsync(materialized, cancellationToken);
            var filteredMaterialized = await this.FilterAsync(sortedMaterialized, cancellationToken);
            return filteredMaterialized;
        }

        protected virtual Task<IQueryable<TSource>> GetSourceQueryAsync(CancellationToken cancellationToken = default)
        {
            var query = this.Inquiry.SourceQuery;
            return Task.FromResult(query);
        }

        protected virtual Task<IQueryable<TSource>> FilterAsync(IQueryable<TSource> queryable, CancellationToken cancellationToken = default)
        {
            if (this.Inquiry.SourceSpecification != null)
            {
                queryable = queryable.Where(this.Inquiry.SourceSpecification);
            }
                
            return Task.FromResult(queryable);
        }

        protected virtual Task<IQueryable<TTarget>> FilterAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            if (this.Inquiry.TargetSpecification != null)
            {
                queryable = queryable.Where(this.Inquiry.TargetSpecification);
            }
                
            return Task.FromResult(queryable);
        }

        protected virtual Task<IQueryable<TSource>> SortAsync(IQueryable<TSource> queryable, CancellationToken cancellationToken = default)
        {
            if (this.Inquiry.SourceSorts != null)
            {
                var effectiveSort = this.Inquiry.SourceSorts.ToArray();
                queryable = queryable.Sort(effectiveSort);
            }

            return Task.FromResult(queryable);
        }

        protected virtual Task<IQueryable<TTarget>> SortAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            if (this.Inquiry.TargetSorts != null)
            {
                var effectiveSort = this.Inquiry.TargetSorts.ToArray();
                queryable = queryable.Sort(effectiveSort);
            }

            return Task.FromResult(queryable);
        }

        protected virtual Task<IQueryable<TTarget>> MaterializeAsync(IQueryable<TSource> queryable, CancellationToken cancellationToken = default)
        {
            var materialized = this.Materializer.Materialize(queryable);
            return Task.FromResult(materialized);
        }

        protected abstract Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default);

    }


}