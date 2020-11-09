namespace Stize.Domain.Entity
{

    public interface IEntity : IObject
    {
    }

    public interface IEntity<TKey> : IEntity, IObject<TKey>
    {

    }
}