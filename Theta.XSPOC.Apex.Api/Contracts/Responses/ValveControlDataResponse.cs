namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// The Valve Control Data Response.
    /// </summary>
    public class ValveControlDataResponse
    {

        /// <summary>
        /// Gets or sets the Value DP.
        /// </summary>
        public string ValueDP { get; set; }

        /// <summary>
        /// Gets or sets the Value SP.
        /// </summary>
        public string ValueSP { get; set; }

        /// <summary>
        /// Gets or sets the Value Flow.
        /// </summary>
        public string ValueFlowRate { get; set; }

        /// <summary>
        /// Gets or sets the Setpoint DP.
        /// </summary>
        public string SetpointDP { get; set; }

        /// <summary>
        /// Gets or sets the Setpoint SP.
        /// </summary>
        public string SetpointSP { get; set; }

        /// <summary>
        /// Gets or sets the Setpoint Flow.
        /// </summary>
        public string SetpointFlowRate { get; set; }

        /// <summary>
        /// Gets or sets the Hi Limit DP.
        /// </summary>
        public string HiLimitDP { get; set; }

        /// <summary>
        /// Gets or sets the Hi Limit SP.
        /// </summary>
        public string HiLimitSP { get; set; }

        /// <summary>
        /// Gets or sets the Hi Limit Flow.
        /// </summary>
        public string HiLimitFlowRate { get; set; }

        /// <summary>
        /// Gets or sets the Lo Limit DP.
        /// </summary>
        public string LoLimitDP { get; set; }

        /// <summary>
        /// Gets or sets the Lo Limit SP.
        /// </summary>
        public string LoLimitSP { get; set; }

        /// <summary>
        /// Gets or sets the Lo Limit Flow.
        /// </summary>
        public string LoLimitFlowRate { get; set; }

        /// <summary>
        /// Gets or sets the DeadBand DP.
        /// </summary>
        public string DeadBandDP { get; set; }

        /// <summary>
        /// Gets or sets the DeadBand SP.
        /// </summary>
        public string DeadBandSP { get; set; }

        /// <summary>
        /// Gets or sets the DeadBand Flow.
        /// </summary>
        public string DeadBandFlowRate { get; set; }

        /// <summary>
        /// Gets or sets the Gain DP.
        /// </summary>
        public string GainDP { get; set; }

        /// <summary>
        /// Gets or sets the Gain SP.
        /// </summary>
        public string GainSP { get; set; }

        /// <summary>
        /// Gets or sets the Gain Flow.
        /// </summary>
        public string GainFlowRate { get; set; }

        /// <summary>
        /// Gets or sets the Shut In Left Value.
        /// </summary>
        public string ShutInLeftValue { get; set; }

        /// <summary>
        /// Gets or sets the SP Override Value.
        /// </summary>
        public string SPOverrideValue { get; set; }

        /// <summary>
        /// Gets or sets the Controller Mode.
        /// </summary>
        public string ControllerModeValue { get; set; }

    }
}
