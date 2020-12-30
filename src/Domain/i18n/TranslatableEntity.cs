using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stize.Domain.Entity;

namespace Stize.Domain.i18n
{
    public abstract class TranslatableEntity<TTranslation, TKey> : BaseEntity<TKey>, ITranslatable<TTranslation, TKey>
        where TTranslation : ITranslation<TKey>
    {
        public ICollection<TTranslation> Translations { get; } = new Collection<TTranslation>();
    }
}