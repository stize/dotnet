namespace Stize.Persistence.QueryResult
{
    public abstract class QueryResult<T> : IQueryResult<T>
    {
        public T Result { get; set; }
    }
}