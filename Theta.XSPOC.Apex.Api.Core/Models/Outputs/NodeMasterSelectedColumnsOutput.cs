using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;
namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for data history list output response.
    /// </summary>
    public class NodeMasterSelectedColumnsOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the Data. 
        /// </summary>
        public Dictionary<string, string> Data { get; set; }

        /// <summary>
        /// Constructor for NodeMasterDictionary.
        /// </summary>
        public NodeMasterSelectedColumnsOutput()
        {
            Data = new Dictionary<string, string>();
        }

    }
}
