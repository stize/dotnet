namespace Stize.CQRS.Query.Domain
{
    public class SingleValueQueryResult<TValue> : IQueryResult
    {
        public TValue Value { get; }
    }
}
