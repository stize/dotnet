namespace Stize.Persistence.InquiryResult
{
    public class PagedInquiryResult<T> : MultipleInquiryResult<T>
        where T : class
    {

        public int Total { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
    }
}