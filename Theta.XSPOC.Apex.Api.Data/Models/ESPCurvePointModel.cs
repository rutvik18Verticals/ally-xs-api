namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the esp curve points model.
    /// </summary>
    public class ESPCurvePointModel
    {

        /// <summary>
        /// Gets or sets the esp pump id.
        /// </summary>
        public int ESPPumpID { get; set; }

        /// <summary>
        /// Gets or sets the flow rate.
        /// </summary>
        public double FlowRate { get; set; }

        /// <summary>
        /// Gets or sets the head feet per stage.
        /// </summary>
        public double HeadFeetPerStage { get; set; }

        /// <summary>
        /// Gets or sets the power in hp.
        /// </summary>
        public double PowerInHP { get; set; }

        /// <summary>
        /// Gets or sets the efficiency.
        /// </summary>
        public double? Efficiency { get; set; }

    }
}
