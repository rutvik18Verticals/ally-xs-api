using Microsoft.EntityFrameworkCore;

namespace Theta.XSPOC.Apex.Api.Data.Entity.Models
{
    /// <summary>
    /// Represents the GetLatestValuesByParamStandardType.
    /// </summary>
    [Keyless]
    public class GetLatestValuesByParamStandardType
    {

        /// <summary>
        /// Get and set the NodeId.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Get and set the NodeId.
        /// </summary>
        public float Value { get; set; }

    }
}
