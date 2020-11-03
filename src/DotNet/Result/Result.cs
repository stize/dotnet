using Stize.DotNet.Result.Reasons;

namespace Stize.DotNet.Result
{

    public class Result : ResultBase<Result>
    {
        private Result(bool success)
        {
            this.Success = success;
        }

        public static Result Ok()
        {
            return new Result(true);
        }

        public static Result Fail(Error error)
        {
            return new Result(false).WithReason(error);
        }
    }

    public class Result<T> : ResultBase<Result<T>>
    {
        public T Value { get; }

        private Result()
        {
            this.Success = false;
        }

        private Result(T value)
        {
            this.Value = value;
            this.Success = true;
        }


        public static Result<T> Ok(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Fail(Error error)
        {
            return new Result<T>().WithReason(error);
        }
    }
}
