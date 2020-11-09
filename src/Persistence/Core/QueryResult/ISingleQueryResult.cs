namespace Stize.Persistence.QueryResult
{
    public interface ISingleQueryResult<out T> : IQueryResult
        where T : class
    {
        T Result { get; }
    }
}