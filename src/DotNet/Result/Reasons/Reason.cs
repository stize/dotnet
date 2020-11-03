namespace Stize.DotNet.Result.Reasons
{
    public abstract class Reason
    {
        public string Message { get; }

        protected Reason(string message)
        {
            this.Message = message;
        }
    }
}