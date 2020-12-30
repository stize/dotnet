using System;

namespace Stize.Domain.Audit
{
    /// <summary>
    /// Interface for SoftDeleted object
    /// </summary>
    public interface ISoftDeleted
    {
        /// <summary>
        /// Boolean that represents if the object is deleted
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Date when the object is deleted
        /// </summary>
        DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Who/What deleted the object
        /// </summary>
        string DeletedBy { get; set; }
    }
}