﻿using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryDispatcher
{
    public interface IQueryDispatcher
    {
        Task<TResult> HandleAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
            where TResult : class, IQueryResult;
    }
}