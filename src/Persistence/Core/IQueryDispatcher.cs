﻿using System.Threading.Tasks;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TSource, TTarget, TResult>(TQuery query)
            where TQuery : IQuery<TSource>
            where TSource : class
            where TTarget : class
            where TResult : IQueryResult;
    }
}