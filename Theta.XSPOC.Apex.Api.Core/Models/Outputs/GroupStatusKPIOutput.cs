using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the output for Group Status KPI.
    /// </summary>
    public class GroupStatusKPIOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of Group Status KPI values.
        /// </summary>
        public List<GroupStatusKPIValues> Values { get; set; }

    }
}
