using Stize.DotNet.Result;

namespace Stize.Persistence.Inquiry
{
    public class MultipleValueInquiry<TSource, TTarget> : Inquiry<TSource, TTarget, MultipleValueResult<TTarget>>
        where TSource : class 
        where TTarget : class

    {
    }

}