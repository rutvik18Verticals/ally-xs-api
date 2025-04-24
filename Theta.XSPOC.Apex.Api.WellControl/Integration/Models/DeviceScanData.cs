using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Represents the device scan data from a device scan call.
    /// </summary>
    public class DeviceScanData : PostProcessingBase
    {

        /// <summary>
        /// Gets or sets the communication status from the firmware call.
        /// </summary>
        public string CommunicationStatusFromFirmwareCall { get; set; }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        public float? FirmwareVersion { get; set; }

        /// <summary>
        /// Gets or sets the poc type.
        /// </summary>
        public int PocType { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the scan time in utc.
        /// </summary>
        public DateTime UtcScanTime { get; set; }

        /// <summary>
        /// Gets or sets the raw data in a dictionary. The key is the address and the value is the scanned value for that
        /// address.
        /// </summary>
        public IDictionary<string, object> Values { get; set; }

    }
}
