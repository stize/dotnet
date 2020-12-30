
using System.Collections.Generic;

namespace Stize.Hosting.AspNetCore.Test.Model
{
    public class TestModel
    {
        public string Property { get; set; }
        public ICollection<string> Collection { get; set; }

        public IEnumerable<TestModel2> ModelCollection { get; set; } = new List<TestModel2>();
    }
}
