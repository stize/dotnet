using System.Collections.Generic;

namespace Stize.DotNet.Result
{
    public class MultipleValueResult<T> : IValueResult<T>
    {
        public IEnumerable<T> Value { get; set; }
    }
}