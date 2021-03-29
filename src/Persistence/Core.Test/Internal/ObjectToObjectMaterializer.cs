using System;
using System.Linq;
using Stize.Persistence.Materializer;

namespace Stize.Persistence.Test.Internal
{
    internal class ObjectToObjectMaterializer<TSource, TTarget> : IMaterializer<TSource, TTarget>
    {
        public IQueryable<TTarget> Materialize(IQueryable<TSource> Inquiryable)
        {
            return Inquiryable.Select(x => Activator.CreateInstance<TTarget>());
        }
    }
}
