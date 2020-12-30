using System;

namespace Stize.DotNet.Result.Reasons
{
    public class ExceptionalError : Error
    {
        public Exception Exception { get; }

        public ExceptionalError(Exception exception, string message) : base(message)
        {
            this.Exception = exception;
        }

        public ExceptionalError(Exception exception) : base(exception.Message)
        {
            this.Exception = exception;
        }
    }
}