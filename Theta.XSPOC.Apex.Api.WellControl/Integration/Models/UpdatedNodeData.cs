using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Represents the fields and values to update for node.
    /// after the run.
    /// </summary>
    public class UpdatedNodeData
    {
        /// <summary>
        /// Describes the full set of data that may be updated on a node.
        /// </summary>
        public enum Fields
        {

            /// <summary>
            /// The field is Enabled.
            /// </summary>
            Enabled,

            /// <summary>
            /// The Data Collection.
            /// </summary>
            DataCollection,

            /// <summary>
            /// The Disable Code.
            /// </summary>
            DisableCode,

            /// <summary>
            /// The Node Id.
            /// </summary>
            NodeId,

        }

        /// <summary>
        /// A dictionary of supported <seealso cref="Fields"/> plus values.
        /// </summary>
        public IDictionary<Fields, object> Values { get; set; }
    }
}
