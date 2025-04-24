namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents a pump configuration.
    /// </summary>
    public class PumpConfigurationModel
    {

        /// <summary>
        /// The configured pump.
        /// </summary>
        public string Pump { get; set; }

        /// <summary>
        /// The stage count associated with the pump.
        /// </summary>
        public int? NumberOfStages { get; set; }

    }
}