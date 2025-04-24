using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The plunger lift trend data model.
    /// </summary>
    public class PlungerLiftTrendDataModel
    {

        /// <summary>
        /// Gets or sets the plunger lift trend date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public float? Value { get; set; }

    }
}
