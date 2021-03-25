using Stize.DotNet.Result.Reasons;

namespace Stize.DotNet.Result
{

    public class Result : ResultBase<Result>, IValueResult
    {
        private Result(bool success)
        {
            this.IsSuccess = success;
        }

        public static Result Success()
        {
            return new Result(true);
        }

        public static Result Fail(Error error)
        {
            return new Result(false).WithReason(error);
        }

        public static Result Fail(string msg)
        {
            return new Result(false).WithReason(new Error(msg));
        }
    }

    public class Result<T> : ResultBase<Result<T>>, IValueResult<T>
    {
        public T Value { get; }

        protected Result()
        {
            this.IsSuccess = false;
        }

        protected Result(T value)
        {
            this.Value = value;
            this.IsSuccess = true;
        }


        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Fail(Error error)
        {
            return new Result<T>().WithReason(error);
        }

        public static Result<T> Fail(string msg)
        {
            return new Result<T>().WithReason(new Error(msg));
        }
    }

    
}
