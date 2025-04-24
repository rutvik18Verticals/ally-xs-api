using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{/// <summary>
 /// Represent the class for Asset details output response.
 /// </summary>
    public class AssetDetailsOutput : CoreOutputBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="AssetDetails"/> values.
        /// </summary>
        public List<AssetDetails> Values { get; set; }
    }
}
