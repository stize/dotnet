using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Inquiry;
using Stize.DotNet.Result;

namespace Stize.Persistence.InquiryHandler
{

    public abstract class PagedValueInquiryHandlerBase<TInquiry, TSource, TTarget, TResult>: InquiryHandler<TInquiry, TSource, TTarget, TResult>
        where TInquiry : PagedValueInquiry<TSource, TTarget>, IInquiry<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : PagedValueResult<TTarget>, new()
    {
        private int count;

        protected PagedValueInquiryHandlerBase(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }

        protected override async Task<IQueryable<TTarget>> ExecuteQueryAsync(TInquiry inquiry, CancellationToken cancellationToken)
        {
            var queryable = await base.ExecuteQueryAsync(inquiry, cancellationToken);

            this.count = await this.Inquiry.Provider.CountAsync(queryable, cancellationToken);

            var paginated = this.Paginate(queryable);

            return paginated;
        }

        protected virtual IQueryable<TTarget> Paginate(IQueryable<TTarget> queryable)
        {
            if (this.Inquiry.Skip.HasValue)
            {
                queryable = queryable.Skip(this.Inquiry.Skip.Value);
            }

            if (this.Inquiry.Take.HasValue)
            {
                queryable = queryable.Take(this.Inquiry.Take.Value);
            }

            return queryable;
        }
        
        protected override async Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var values = await this.Inquiry.Provider.ToArrayAsync(queryable, cancellationToken);
            var result = new TResult
            {
                Value = values,
                Total = this.count,
                Take = this.Inquiry.Take,
                Skip = this.Inquiry.Skip
            };
            return result;
        }
    }

    public class PagedValueInquiryHandler<TSource, TTarget>: PagedValueInquiryHandlerBase<PagedValueInquiry<TSource, TTarget>, TSource, TTarget, PagedValueResult<TTarget>>
        where TSource : class
        where TTarget : class
    {
        public PagedValueInquiryHandler(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }
    }

   
}