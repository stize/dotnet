namespace Stize.Persistence.InquiryResult
{
    public class SingleInquiryResult<T> : InquiryResult<T>
        where T : class
    {
        public T Result { get; set; }
    }
}