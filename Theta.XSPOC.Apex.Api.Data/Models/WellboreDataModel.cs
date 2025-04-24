using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{

    /// <summary>
    /// Represents the wellbore data model.
    /// </summary>
    public class WellboreDataModel
    {

        /// <summary>
        /// Gets or sets the Perforations.
        /// </summary>
        public IList<PerforationModel> Perforations { get; set; }

        /// <summary>
        /// Gets or sets the packer depth.
        /// </summary>
        public float? PackerDepth { get; set; }

        /// <summary>
        /// Gets or sets the production depth.
        /// </summary>
        public float? ProductionDepth { get; set; }

        /// <summary>
        /// Gets or sets the has packer.
        /// </summary>
        public bool? HasPacker { get; set; }

        /// <summary>
        /// Gets or sets the Tubings.
        /// </summary>
        public IList<TubingModel> Tubings { get; set; }

    }
}
