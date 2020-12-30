namespace Stize.Domain
{

    public interface IObject 
    {
    }

    public interface IObject<TKey> : IObject
    {
        TKey Id { get; set; }
    }
}