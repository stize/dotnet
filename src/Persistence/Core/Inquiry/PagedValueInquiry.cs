using Stize.DotNet.Result;

namespace Stize.Persistence.Inquiry
{
    public class PagedValueInquiry<TSource, TTarget> : Inquiry<TSource, TTarget, PagedValueResult<TTarget>>
        where TSource : class
        where TTarget : class

    {
        public int? Take { get; set; }

        public int? Skip { get; set; }

    }
}