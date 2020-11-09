using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Search.Page;
using Stize.DotNet.Search.Sort;

namespace Stize.Hosting.AspNetCore.Model
{
    public class PageDescriptorModel : IPageDescriptor
    {
        public const string SkipKey = "skip";
        public const string TakeKey = "take";
        public const string EnvelopeKey = "envelope";
        public const string FilterKey = "filters";
        public const string SortKey = "sorts";

        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool? Envelope { get; set; }

        public ICollection<FilterDescriptor> Filters { get; set; } = new Collection<FilterDescriptor>();
        public ICollection<SortDescriptor> Sorts { get; set; } = new Collection<SortDescriptor>();


    }
}