using System;

namespace Stize.Domain.Audit
{
    /// <summary>
    /// Interface for an audited object
    /// </summary>
    public interface IAudited
    {
        /// <summary>
        /// Date when the object was created
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Who/What creates the object
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// Date when the object was updated for last time
        /// </summary>
        DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Who/What updates the object for last time
        /// </summary>
        string UpdatedBy { get; set; }
    }
}