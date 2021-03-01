using Stize.Persistence.InquiryResult;

namespace Stize.Persistence.Inquiry
{
    public class MultipleValueInquiry<TSource, TTarget> : Inquiry<TSource, TTarget, MultipleInquiryResult<TTarget>>
        where TSource : class 
        where TTarget : class

    {
    }

}