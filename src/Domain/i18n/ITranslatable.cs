using System.Collections.Generic;

namespace Stize.Domain.i18n
{
    public interface ITranslatable<TTranslation, TKey> : IEntity<TKey>
        where TTranslation : ITranslation<TKey>
    {
        ICollection<TTranslation> Translations { get; }
    }
}