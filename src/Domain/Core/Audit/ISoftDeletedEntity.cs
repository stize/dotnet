using System;

namespace Stize.Domain.Audit
{
    /// <summary>
    /// Interface for SoftDeletedEntity
    /// </summary>
    public interface ISoftDeletedEntity
    {
        /// <summary>
        /// Boolean that represents if the entity is deleted
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Date when the entity is deleted
        /// </summary>
        DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Who/What deleted the entity
        /// </summary>
        string DeletedBy { get; set; }
    }
}