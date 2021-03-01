using System.Collections.Generic;

namespace Stize.Persistence.InquiryResult
{
    public class MultipleInquiryResult<T> : InquiryResult<T>
        where T : class
    {
        public IEnumerable<T> Result { get; set; }
    }
}