namespace Stize.Domain
{
    /// <summary>
    /// Core Domain Entity implementation
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
    }

    /// <summary>
    /// Keyed Core Domain Entity implementation
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public abstract class BaseEntity<K> : BaseEntity, IEntity<K>
    {
        /// <summary>
        /// Entity Primary Key
        /// </summary>
        public virtual K Id { get; set; }
    }
}