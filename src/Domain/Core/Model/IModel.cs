namespace Stize.Domain.Model
{

    public interface IModel: IObject
    {
    }

    public interface IModel<TKey> : IModel, IObject<TKey>
    {
        
    }
}