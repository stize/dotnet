using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Inquiry;
using Stize.Persistence.InquiryResult;

namespace Stize.Persistence.InquiryDispatcher
{
    public interface IInquiryDispatcher
    {
        Task<TResult> HandleAsync<TResult>(IInquiry<TResult> inquiry, CancellationToken cancellationToken = default)
            where TResult : class, IInquiryResult;
    }
}