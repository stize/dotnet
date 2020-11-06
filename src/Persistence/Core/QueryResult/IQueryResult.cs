namespace Stize.Persistence.QueryResult
{
    /// <summary>
    /// Query result interface
    /// </summary>
    public interface IQueryResult
    {
    }

    /// <summary>
    /// Query result interface
    /// </summary>
    /// <typeparam name="T">Result type</typeparam>
    public interface IQueryResult<T> : IQueryResult
        where T : class
    {
    }
}