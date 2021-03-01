using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Inquiry;
using Stize.Persistence.InquiryResult;

namespace Stize.Persistence.InquiryHandler
{
    public abstract class MultipleValueInquiryHandlerBase<TInquiry, TSource, TTarget, TResult> : InquiryHandler<TInquiry, TSource, TTarget, TResult>
        where TInquiry : MultipleValueInquiry<TSource, TTarget>, IInquiry<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : MultipleInquiryResult<TTarget>, new()
    {
        protected MultipleValueInquiryHandlerBase(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }

        protected override async Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var values = await this.Inquiry.Provider.ToArrayAsync(queryable, cancellationToken);
            var result = new TResult { Result = values };
            return result;
        }
    }

    public class MultipleValueInquiryHandler<TSource, TTarget> : MultipleValueInquiryHandlerBase<MultipleValueInquiry<TSource, TTarget>, TSource, TTarget, MultipleInquiryResult<TTarget>>
        where TSource : class
        where TTarget : class
    {
        public MultipleValueInquiryHandler(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }

    }
}