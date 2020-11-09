namespace Stize.DotNet.Result.Reasons
{
    public abstract class Reason
    {
        public string Message { get; }
        public string Kind { get; }

        protected Reason(string message, string kind)
        {
            this.Message = message;
            this.Kind = kind;
        }
    }
}