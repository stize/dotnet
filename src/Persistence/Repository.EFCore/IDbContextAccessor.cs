using Microsoft.EntityFrameworkCore;

namespace Stize.Persistence.Repository.EntityFrameworkCore
{
    public interface IDbContextAccessor
    {
        TContext GetCurrentContext<TContext>() where TContext : DbContext;
    }
}