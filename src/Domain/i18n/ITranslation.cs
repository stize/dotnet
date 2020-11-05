namespace Stize.Domain.i18n
{
    public interface ITranslation<TKey>
    {
        TKey TranslatableId { get; }

        string Culture { get; }
    }
}