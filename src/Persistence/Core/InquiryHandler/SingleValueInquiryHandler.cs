using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Inquiry;
using Stize.DotNet.Result;

namespace Stize.Persistence.InquiryHandler
{
    public abstract class SingleValueInquiryHandlerBase<TInquiry, TSource, TTarget, TResult> : InquiryHandler<TInquiry, TSource, TTarget, TResult>
        where TInquiry: SingleValueInquiry<TSource, TTarget>, IInquiry<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : SingleValueResult<TTarget>, new()
    {
        protected SingleValueInquiryHandlerBase(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }

        protected override async Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var value = await this.Inquiry.Provider.SingleOrDefaultAsync(queryable, cancellationToken);
            var result = new TResult() { Value = value };
            return result;
        }
    }

    public class SingleValueInquiryHandler<TSource, TTarget> : SingleValueInquiryHandlerBase<SingleValueInquiry<TSource, TTarget>, TSource, TTarget, SingleValueResult<TTarget>>
        where TSource : class
        where TTarget : class
    {
        public SingleValueInquiryHandler(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }
        
        
    }

}