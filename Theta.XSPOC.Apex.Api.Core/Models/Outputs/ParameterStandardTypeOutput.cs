using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{/// <summary>
 /// Represent the class for data history trend output response.
 /// </summary>
    public class ParameterStandardTypeOutput : CoreOutputBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="ParamStandardTypeDetails"/> values.
        /// </summary>
        public List<ParamStandardTypeDetails> Values { get; set; }
    }
}
