using System;
using System.ComponentModel;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The Measurement Trend Data Model.
    /// </summary>
    public class MeasurementTrendDataModel
    {

        /// <summary>
        /// Gets or sets the Date.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the Date.
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>  
        [DefaultValue(false)]
        public bool IsManual { get; set; }
    }
}
