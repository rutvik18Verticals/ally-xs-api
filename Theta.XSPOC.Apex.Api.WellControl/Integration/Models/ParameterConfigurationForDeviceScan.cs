namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Represents the data needed for a parameter to participate in a device scan
    /// </summary>
    public class ParameterConfigurationForDeviceScan
    {

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets the collection mode.
        /// </summary>
        public CollectionMode CollectionMode { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the earliest firmware version that this parameter is supported in.
        /// </summary>
        public decimal EarliestSupportedVersion { get; set; }

        /// <summary>
        /// Gets or sets the param standard type.
        /// </summary>
        public int? ParamStandardType { get; set; }

        /// <summary>
        /// Gets or sets the POC type.
        /// </summary>
        public int PocType { get; set; }

        /// <summary>
        /// Gets or sets whether this parameter is included in a fast scan.
        /// </summary>
        public bool IncludeInFastScan { get; set; }

        /// <summary>
        /// Gets or sets whether the parameter represents a setpoint.
        /// </summary>
        public bool IsSetpoint { get; set; }

        /// <summary>
        /// Gets or sets whether the parameter is included in a status scan.
        /// </summary>
        public bool IsStatusScan { get; set; }

    }
}
