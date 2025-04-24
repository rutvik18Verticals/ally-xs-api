using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.WellControl.Contracts.Responses
{

    /// <summary>
    /// Describes a setpoints group data response that needs to be send out.
    /// </summary>
    public class SetpointsGroupDataResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<SetpointsGroupsData> Values { get; set; }

    }
}
