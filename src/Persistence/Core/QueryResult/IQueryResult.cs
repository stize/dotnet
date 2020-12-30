using System;

namespace Stize.Persistence.QueryResult
{
    public interface IQueryResult
    {
    }

    public interface IQueryResult<T> : IQueryResult
    {
        T Result { get; set; }
    }
}