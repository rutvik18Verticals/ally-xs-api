using System.Collections.Generic;
namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represent the class for data history list output response.
    /// </summary>
    public class NodeMasterSelectedColumnsOutput
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
