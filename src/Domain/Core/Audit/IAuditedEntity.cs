using System;

namespace Stize.Domain.Audit
{
    /// <summary>
    /// Interface for an audited entity
    /// </summary>
    public interface IAuditedEntity
    {
        /// <summary>
        /// Date when the entity was created
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Who/What creates the entity
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// Date when the entity was updated for last time
        /// </summary>
        DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Who/What updates the entity for last time
        /// </summary>
        string UpdatedBy { get; set; }
    }
}