using Microsoft.EntityFrameworkCore;

namespace Theta.XSPOC.Apex.Api.Data.Entity.Models
{
    /// <summary>
    /// Represents the GetLatestValuesStateByParamStandardType.
    /// </summary>
    [Keyless]
    public class GetLatestValuesStateByParamStandardType
    {

        /// <summary>
        /// Get and set the NodeId.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Get and set the Value.
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Get and set the Text.
        /// </summary>
        public string Text { get; set; }

    }
}
