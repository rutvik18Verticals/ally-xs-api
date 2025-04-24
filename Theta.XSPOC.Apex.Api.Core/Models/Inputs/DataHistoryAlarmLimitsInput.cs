using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// This represents the Data History Alarm Limit Input.
    /// </summary>
    public class DataHistoryAlarmLimitsInput
    {

        /// <summary>
        /// The asset id.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// The addresses.
        /// </summary>
        public string[] Addresses { get; set; }

    }
}
