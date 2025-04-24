using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The analysis correlations data model.
    /// </summary>
    public class NodeMasterDictionary
    {

        /// <summary>
        /// Gets or sets the Data. 
        /// </summary>
        public Dictionary<string, string> Data { get; set; }

        /// <summary>
        /// Constructor for NodeMasterDictionary.
        /// </summary>
        public NodeMasterDictionary()
        {
            Data = new Dictionary<string, string>();
        }
    }
}
