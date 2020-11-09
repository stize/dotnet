using System.Collections.Generic;

namespace Stize.Hosting.AspNetCore.Model
{
    public class EnvelopedModel<T>
    {
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}