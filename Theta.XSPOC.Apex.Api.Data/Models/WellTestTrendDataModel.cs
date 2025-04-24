using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the well test trend data model.
    /// </summary>
    public class WellTestTrendDataModel
    {

        /// <summary>
        /// Gets or sets the well test date.
        /// </summary>
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public float? Value { get; set; }

    }
}
