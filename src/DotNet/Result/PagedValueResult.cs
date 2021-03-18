namespace Stize.DotNet.Result
{
    public class PagedValueResult<T> : MultipleValueResult<T>
    {

        public int Total { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
    }
}