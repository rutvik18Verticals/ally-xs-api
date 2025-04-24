using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The ProductionStatisticsTrendDataValue.
    /// </summary>
    public class ProductionStatisticsTrendDataModel
    {

        /// <summary>
        /// Get and set the Processed Date.
        /// </summary>
        public DateTime? ProcessedDate { get; set; }

        /// <summary>
        /// Get and set the Value.
        /// </summary>
        public float? Value { get; set; }

    }
}
