using Stize.Persistence.InquiryResult;

namespace Stize.Persistence.Inquiry
{
    public class SingleValueInquiry<TSource, TTarget> : Inquiry<TSource, TTarget, SingleInquiryResult<TTarget>>
        where TSource : class 
        where TTarget : class

    {
    }

}