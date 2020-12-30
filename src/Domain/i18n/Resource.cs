using System.ComponentModel.DataAnnotations;

namespace Stize.Domain.i18n
{
    public class Resource
    {
        [StringLength(5)]
        public string Culture { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
       
    }
}