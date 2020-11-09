namespace Stize.Domain.Model
{
    public abstract class BaseModel : IModel
    {
    }


    public abstract class BaseModel<TKey> : BaseModel, IModel<TKey>
    {
        public virtual TKey Id { get; set; }
    }
}