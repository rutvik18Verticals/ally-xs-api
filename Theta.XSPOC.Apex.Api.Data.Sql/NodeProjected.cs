using System;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// Represents a projected node object used in the select of the linq statement.
    /// </summary>
    public class NodeProjected
    {

        /// <summary>
        /// Gets or sets the asset GUID.
        /// </summary>
        public Guid AssetGUID { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the poc type.
        /// </summary>
        public int PocType { get; set; }

        /// <summary>
        /// Gets or sets the company guid.
        /// </summary>
        public Guid? CustomerGUID { get; set; }

    }
}