using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Represent the class for Node Master Columns Input input data.
    /// </summary>
    public class NodeMasterColumnsInput
    {

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        public string[] Columns { get; set; }

    }
}
