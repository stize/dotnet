namespace Stize.Persistence.QueryResult
{
    public class SingleQueryResult<T> : QueryResult<T>
        where T : class
    {
        public T Result { get; set; }
    }
}