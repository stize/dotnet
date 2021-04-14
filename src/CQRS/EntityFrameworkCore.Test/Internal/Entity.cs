using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stize.Domain.Entity;

namespace Stize.CQRS.EntityFrameworkCore.Test.Internal
{
    public class Entity: BaseEntity<int>
    {
    }

    public class EntityConfiguration : IEntityTypeConfiguration<Entity>
    {
        public void Configure(EntityTypeBuilder<Entity> builder)
        {
        }
    }

}
