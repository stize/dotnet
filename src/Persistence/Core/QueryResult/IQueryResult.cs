using System;

namespace Stize.Persistence.QueryResult
{
    public interface IQueryResult
    {
    }

    public interface IQueryResult<out TTarget> : IQueryResult
    {

    }
}