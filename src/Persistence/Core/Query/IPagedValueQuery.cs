using System.Collections.Generic;
using Stize.DotNet.Search.Sort;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Query
{
    /// <summary>
    /// Paged Value Query interface. Inherits from IQuery
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TResult">QueryResult type</typeparam>
    public interface IPagedValueQuery<TEntity, TResult> : IQuery<TEntity, IPagedQueryResult<TResult>> 
        where TEntity : class 
        where TResult : class
    {
        int? Take { get; }
        int? Skip { get; }

        public IEnumerable<SortDescriptor> Sorts { get; }
    }
}