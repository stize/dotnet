namespace Stize.DotNet.Result
{

    public class SingleValueResult<T> : IValueResult<T>
    {
        public T Value { get; set; }
    }
}