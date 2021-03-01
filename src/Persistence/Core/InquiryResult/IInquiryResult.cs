namespace Stize.Persistence.InquiryResult
{
    public interface IInquiryResult
    {
    }

    public interface IInquiryResult<out TTarget> : IInquiryResult
    {

    }
}