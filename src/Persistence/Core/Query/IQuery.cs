using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.DotNet.Specification;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Query
{
    /// <summary>
    /// Query interface
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TResult">QueryResult type</typeparam>
    public interface IQuery<TEntity, TResult>
        where TEntity : class
        where TResult : IQueryResult
    {
        ISpecification<TEntity> Specification { get; }
    }
}