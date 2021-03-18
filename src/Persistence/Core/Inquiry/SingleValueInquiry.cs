using Stize.DotNet.Result;

namespace Stize.Persistence.Inquiry
{
    public class SingleValueInquiry<TSource, TTarget> : Inquiry<TSource, TTarget, SingleValueResult<TTarget>>
        where TSource : class 
        where TTarget : class

    {
    }

}