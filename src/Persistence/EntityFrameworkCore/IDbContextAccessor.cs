using Microsoft.EntityFrameworkCore;

namespace Stize.Persistence.EntityFrameworkCore
{
    public interface IDbContextAccessor
    {
        TContext GetCurrentContext<TContext>() where TContext : DbContext;
    }
}