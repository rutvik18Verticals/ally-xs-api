using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The analysis trend data model.
    /// </summary>
    public class AnalysisTrendDataModel
    {

        /// <summary>
        /// Gets or sets the analysis trend date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public float? Value { get; set; }

    }
}