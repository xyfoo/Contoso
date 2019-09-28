using System;

namespace Contoso.Core.Models.Interfaces
{
    /// <summary>
    /// Interface for classes which perform date-based calculation.
    /// </summary>
    public interface ITestableDate
    {
        /// <summary>
        /// Reference date on which date-based calculation is based on.
        /// </summary>
        DateTimeOffset ReferenceDate { get; set; }
    }
}
