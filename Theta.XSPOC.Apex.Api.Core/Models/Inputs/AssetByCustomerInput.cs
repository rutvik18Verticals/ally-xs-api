
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// This represents the Assets Input.
    /// </summary>
    public class AssetByCustomerInput
    {
        /// <summary>
        /// List of CustomerIds
        /// </summary>      
        public List<string> CustomerIds { get; set; }
    }

}
