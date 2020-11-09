namespace Stize.Persistence.QueryResult
{
    public class SingleQueryResult<T> : ISingleQueryResult<T> 
        where T : class
    {
        public SingleQueryResult(T result)
        {
            this.Result = result;
        }

        public T Result { get; }
    }
}