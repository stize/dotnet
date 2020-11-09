namespace Stize.Domain.Entity
{

    public abstract class BaseEntity : IEntity
    {
    }

    public abstract class BaseEntity<TKey> : BaseEntity, IEntity<TKey>
    {
       
        public virtual TKey Id { get; set; }
    }
}