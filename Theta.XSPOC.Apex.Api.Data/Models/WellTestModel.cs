using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the well test data model.
    /// </summary>
    public class WellTestModel
    {

        /// <summary>
        /// Gets or sets whether the well test is approved.
        /// </summary>
        public bool? Approved { get; set; }

        /// <summary>
        /// Gets or sets the gas rate.
        /// </summary>
        public float? GasRate { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the well test date.
        /// </summary>
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Gets or sets the oil rate.
        /// </summary>
        public float? OilRate { get; set; }

        /// <summary>
        /// Gets or sets the water rate.
        /// </summary>
        public float? WaterRate { get; set; }

        /// <summary>
        /// Gets or sets the fluid above pump.
        /// </summary>
        public float? FluidAbovePump { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public float? Duration { get; set; }

    }
}
