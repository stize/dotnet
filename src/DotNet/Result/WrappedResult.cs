using Stize.DotNet.Result.Reasons;

namespace Stize.DotNet.Result
{

    public class WrappedResult : WrappedResultBase<WrappedResult>
    {
        private WrappedResult(bool success)
        {
            this.Success = success;
        }

        public static WrappedResult Ok()
        {
            return new WrappedResult(true);
        }

        public static WrappedResult Fail(Error error)
        {
            return new WrappedResult(false).WithReason(error);
        }
    }

    public class WrappedResult<T> : WrappedResultBase<WrappedResult<T>>
    {
        public T Value { get; }

        protected WrappedResult()
        {
            this.Success = false;
        }

        protected WrappedResult(T value)
        {
            this.Value = value;
            this.Success = true;
        }


        public static WrappedResult<T> Ok(T value)
        {
            return new WrappedResult<T>(value);
        }

        public static WrappedResult<T> Fail(Error error)
        {
            return new WrappedResult<T>().WithReason(error);
        }
    }

    
}
