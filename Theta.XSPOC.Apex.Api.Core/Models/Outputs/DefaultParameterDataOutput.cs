
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// This is a record that represents the default parameter data.
    /// </summary>
    public class DefaultParameterDataOutput : CoreOutputBase
    {
        /// <summary>
        /// Gets or sets the default paramters list.
        /// </summary>
        public List<DefaultParameterDataModel> DefaultParameters { get; set; }
    }
}
