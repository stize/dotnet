namespace Stize.Persistence.Query
{
    public interface ISingleValueQuery<T> : IQuery<T>
        where T : class
    {
    }
}