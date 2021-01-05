using System.Collections.Generic;
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

        protected Result()
        {
            this.Success = false;
        }

        protected Result(T value)
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

    public class PaginatedResult<T> : ResultBase<PaginatedResult<T>>
    {
        public IEnumerable<T> Value { get; }
        public int? Take { get; protected set; }
        public int? Skip { get; protected set; }
        public int Total { get; protected set; }

        protected PaginatedResult()
        {
            this.Success = false;
        }

        protected PaginatedResult(IEnumerable<T> value)
        {
            this.Value = value;
            this.Success = true;
        }

        public static PaginatedResult<T> Ok(IEnumerable<T> value)
        {
            return new PaginatedResult<T>(value);
        }

        public static PaginatedResult<T> Fail(Error error)
        {
            return new PaginatedResult<T>().WithReason(error);
        }

        public PaginatedResult<T> WithTake(int? take)
        {
            this.Take = take;
            return this;
        }
       
        public PaginatedResult<T> WithSkip(int? skip)
        {
            this.Skip = skip;
            return this;
        }
        
        public PaginatedResult<T> WithTotal(int total)
        {
            this.Total = total;
            return this;
        }
    }
}
