namespace Stize.DotNet.Result.Reasons
{
    public class Warning : Reason
    {
        public Warning(string message) : base(message, nameof(Warning))
        {
        }
    }
}