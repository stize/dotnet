using System.Collections.Generic;

namespace Stize.Domain.Model
{
    public class EnvelopedModel<T> : IEnvelopedModel<T>
    {
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}