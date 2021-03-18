using System.Threading;
using System.Threading.Tasks;
using Stize.DotNet.Result;
using Stize.Persistence.Inquiry;

namespace Stize.Persistence.InquiryDispatcher
{
    public interface IInquiryDispatcher
    {
        Task<TResult> HandleAsync<TResult>(IInquiry<TResult> inquiry, CancellationToken cancellationToken = default)
            where TResult : class, IValueResult;
    }
}