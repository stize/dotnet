using System.ComponentModel.DataAnnotations;

namespace Stize.Domain.i18n
{
    public abstract class TranslationEntity<TKey> : ITranslation<TKey>
    {
        [Required]
        public TKey TranslatableId { get; set; }

        [Required]
        [StringLength(5)]
        public string Culture { get; set; }
    }
}